using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using SharpWeld.Attributes;
using System.Runtime.InteropServices;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace SharpWeld.ClassProvider
{
    public class AssemblyOpCodeBuilder
    {
        private static int _guid = 0;

        //a cache so that each object will not be compiled into a new assembly everytime
        private static Dictionary<Type, Object[]> assemblyCache = new Dictionary<Type, Object[]>();

        private Object[] GetAssemblyForType(Type type)
        {
            try
            {
                return assemblyCache[type];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return null;
            }
        }

        public T BuildAssemblyFromType<T>(Type type, Object [] args)
        {
            Object[] assemblyAndType = GetAssemblyForType(type);

            if (assemblyAndType != null)
            {
                return CreateInstance<T>((System.Reflection.Emit.AssemblyBuilder)assemblyAndType[0], assemblyAndType[1].ToString(), args);
            }

            AppDomain domain = Thread.GetDomain();
            AssemblyName name = new AssemblyName(Utilities.RandomString.Generate());
            System.Reflection.Emit.AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(name, System.Reflection.Emit.AssemblyBuilderAccess.Run);
            ModuleBuilder modBuilder = assemblyBuilder.DefineDynamicModule(name.Name, true);
            string newTypeName = type.Name + "_" + (_guid++);
            TypeBuilder typeBuilder = modBuilder.DefineType(newTypeName, new TypeAttributes(), type);
            
            FieldBuilder memberField = null;
            foreach(MethodInfo method in type.GetMethods())
            {
                if (method.IsAbstract && !method.IsSpecialName)
                {
                    memberField = BuildDiscreteMethod(typeBuilder, method);

                    MethodBuilder overrideBody = GetMethodOverride(typeBuilder, method, memberField);
                    typeBuilder.DefineMethodOverride(overrideBody, method);

                }
            }
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if ((propertyInfo.CanRead && propertyInfo.GetGetMethod().IsAbstract) || (propertyInfo.CanWrite && propertyInfo.GetSetMethod().IsAbstract))
                {
                    memberField = CreateMemberField(typeBuilder, propertyInfo);
                    typeBuilder.DefineProperty(propertyInfo.Name,
                                                         PropertyAttributes.HasDefault,
                                                         propertyInfo.PropertyType,
                                                         null);
                    if (propertyInfo.CanRead)
                    {
                        GetGetMethodBuilderFromProperty(typeBuilder, propertyInfo, memberField);
                    }

                    if (propertyInfo.CanWrite)
                    {
                        GetSetMethodBuilderFromProperty(typeBuilder, propertyInfo, memberField);
                    }
                    else
                    {
                        typeBuilder.DefineProperty("__" + propertyInfo.Name,
                                                         PropertyAttributes.HasDefault,
                                                         propertyInfo.PropertyType,
                                                         null);
                        GetDiscreteSetMethodBuilderFromProperty(typeBuilder, propertyInfo, memberField);
                    }
                }
            }
            
            Type newType = typeBuilder.CreateType();

            //cache it
            assemblyAndType = new Object[2];
            assemblyAndType[0] = assemblyBuilder;
            assemblyAndType[1] = newType.FullName;
            assemblyCache.Add(type, assemblyAndType);

            return CreateInstance<T>(assemblyBuilder, newType.FullName, args);
        }

        private T CreateInstance<T>(System.Reflection.Emit.AssemblyBuilder assembly, string className, Object[] args)
        {
            return (T)assembly.CreateInstance(className, false, BindingFlags.CreateInstance, null, args, null, null);
        }

        private FieldBuilder CreateMemberField(TypeBuilder typeBuilder, PropertyInfo propertyInfo)
        {
            return typeBuilder.DefineField("_" + propertyInfo.Name, propertyInfo.PropertyType, FieldAttributes.Private);
        }

        private MethodBuilder GetGetMethodBuilderFromProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldBuilder fieldBuilder)
        {
            MethodBuilder retVal = typeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.HideBySig, propertyInfo.PropertyType, new Type[0]);
            ILGenerator gen = retVal.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, fieldBuilder);
            gen.Emit(OpCodes.Ret);
            return retVal;
        }

        private MethodBuilder GetDiscreteSetMethodBuilderFromProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldBuilder fieldBuilder)
        {
            MethodBuilder retVal = typeBuilder.DefineMethod("set___" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[1] { propertyInfo.PropertyType });
            ILGenerator gen = retVal.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, fieldBuilder);
            gen.Emit(OpCodes.Ret);
            return retVal;
        }

        private MethodBuilder GetSetMethodBuilderFromProperty(TypeBuilder typeBuilder, PropertyInfo propertyInfo, FieldBuilder fieldBuilder)
        {
            MethodBuilder retVal = typeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[1] { propertyInfo.PropertyType });
            ILGenerator gen = retVal.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, fieldBuilder);
            gen.Emit(OpCodes.Ret);
            return retVal;
        }

        private FieldBuilder BuildDiscreteMethod(TypeBuilder typeBuilder, MethodInfo methodInfo)
        {
            //need to create the private member first
            //ex: private System.Func<System.Int32,System.Int32,System.Int32,System.Int32,System.Double> _Subtract;
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + methodInfo.Name, GetDelegateFuncType(methodInfo), FieldAttributes.Private);

            //define the function delegate now
            //ex: public void __Subtract(System.Func<System.Int32,System.Int32,System.Int32,System.Int32,System.Double> value) 
            //{ 
            //    _Subtract = value; 
            //}
            MethodBuilder discreteMethod = typeBuilder.DefineMethod("__" + methodInfo.Name, MethodAttributes.Public, typeof(void), new Type[1]{GetDelegateFuncType(methodInfo)});
            ILGenerator gen = discreteMethod.GetILGenerator();
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, fieldBuilder);
            gen.Emit(OpCodes.Ret);

            return fieldBuilder;
        }

        private Type GetDelegateFuncType(MethodInfo methodInfo)
        {
            List<Type> types = GetListOfMethodParameters(methodInfo);
            types.Add(methodInfo.ReturnType);
            return GetFunctionType(types.Count).MakeGenericType(types.ToArray());
        }

        private Type GetFunctionType(int numberOfArgs)
        {
            //This makes no sense, but you need to specify how many args to a Func,
            //but there is no dynamic way to define this at runtime, so here's the crazy hack for it
            switch (numberOfArgs)
            {
                case 1:
                    return typeof(Func<>);
                case 2:
                    return typeof(Func<,>);
                case 3:
                    return typeof(Func<,,>);
                case 4:
                    return typeof(Func<,,,>);
                case 5:
                    return typeof(Func<,,,,>);
                /*case 6:
                    return typeof(Func<,,,,,>);
                case 7:
                    return typeof(Func<,,,,,,>);
                case 8:
                    return typeof(Func<,,,,,,,>);
                case 9:
                    return typeof(Func<,,,,,,,,>);
                case 10:
                    return typeof(Func<,,,,,,,,,>);
                case 11:
                    return typeof(Func<,,,,,,,,,,>);
                case 12:
                    return typeof(Func<,,,,,,,,,,,>);
                case 13:
                    return typeof(Func<,,,,,,,,,,,,>);
                case 14:
                    return typeof(Func<,,,,,,,,,,,,,>);
                case 15:
                    return typeof(Func<,,,,,,,,,,,,,,>);
                case 16:
                    return typeof(Func<,,,,,,,,,,,,,,,>);
                case 17:
                    return typeof(Func<,,,,,,,,,,,,,,,,>);*/
                default:
                    throw new ArgumentException("Too many arguments. TODO: better error information");
            }
        }

        private MethodBuilder GetMethodOverride(TypeBuilder tb, MethodInfo methodInfo, FieldBuilder delegateName)
        {
            
            //ex: public override System.Double Subtract (System.Int32 arg1,System.Int32 arg2,System.Int32 arg3,System.Int32 arg4) 
            //{
            //    return _Subtract(arg1,arg2,arg3,arg4);
            //}
            Type[] list = GetMethodParameters(methodInfo);
            MethodBuilder retVal = tb.DefineMethod(methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, methodInfo.ReturnType, list);
            ILGenerator gen = retVal.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, delegateName);
            if (list.Length > 0) gen.Emit(OpCodes.Ldarg_1);
            if (list.Length > 1) gen.Emit(OpCodes.Ldarg_2);
            if (list.Length > 2) gen.Emit(OpCodes.Ldarg_3);
            if (list.Length > 3)
            {
                for (byte i = 4; i < list.Length + 1; i++)
                {
                    gen.Emit(OpCodes.Ldarg_S, i);
                }
            }
            gen.Emit(OpCodes.Tailcall);
            gen.Emit(OpCodes.Callvirt, GetDelegateFuncType(methodInfo).GetMethod("Invoke"));
            gen.Emit(OpCodes.Ret);

            return retVal;
        }

        private Type[] GetMethodParameters(MethodInfo methodInfo)
        {
            return GetListOfMethodParameters(methodInfo).ToArray();
        }

        private List<Type> GetListOfMethodParameters(MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            List<Type> list = new List<Type>(parameters.Length);
            foreach (ParameterInfo param in parameters)
            {
                list.Add(param.ParameterType);
            }
            return list;
        }
    }
}