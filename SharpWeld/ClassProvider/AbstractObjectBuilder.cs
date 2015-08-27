using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpWeld.ClassProvider
{
    /// <summary>
    /// This class isn't actually abstract, strangely enough.  It's intention is to
    /// take a base abstract class, create and instance of it and return the new
    /// instance with the abstract properties and methods overridden.
    /// </summary>
    public class AbstractObjectBuilder<T>
    {
        private const string KEYWORD_NAMESPACE = "namespace";
        private const string KEYWORD_PUBLIC = "public";
        private const string KEYWORD_PRIVATE = "private";
        private const string KEYWORD_CLASS = "class";
        private const string KEYWORD_RETURN = "return";
        private const string KEYWORD_OVERRIDE = "override";
        private const string KEYWORD_SEALED = "sealed";
        private const string KEYWORD_VOID = "void";
        private const string PREFIX_PRIVATE = "_";
        private const string PREFIX_HIDDEN = "__";

        private Builder builder;

        private string _namespace;
        private string _classname;

        private StringBuilder classSource;

        private static long _guid = 0;

        //a cache so that each object will not be compiled into a new assembly everytime
        private static Dictionary<Type, Object[]> assemblyCache = new Dictionary<Type, Object[]>();

        public AbstractObjectBuilder(Builder value)
        {
            builder = value;
            classSource = new StringBuilder();
        }

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

        public T Construct(Type type, Object [] args)
        {
            Object[] assemblyAndType = GetAssemblyForType(type);
            GenerateClassName(type);

            if (assemblyAndType != null)
            {
                return CreateInstance((Assembly) assemblyAndType[0], assemblyAndType[1].ToString(), args);
            }

            _classname += "_" + (_guid++);// create a unique class name, I don't remember why

            classSource.Append(KEYWORD_NAMESPACE + " " + this._namespace + " {");
            classSource.Append(GetClassDefinition(type));
            classSource.Append(OverrideAbstractProperties(type));
            classSource.Append(OverrideAbstractMethods(type));
            classSource.Append("}");
            classSource.Append("}");

            Assembly assembly = builder.BuildAssembly(classSource.ToString(), type);
            //cache it
            assemblyAndType = new Object[2];
            assemblyAndType[0] = assembly;
            assemblyAndType[1] = GetFQAN(assembly);
            assemblyCache.Add(type, assemblyAndType);
            
            return CreateInstance(assembly, GetFQAN(assembly), args);
        }

        private T CreateInstance(Assembly assembly, string className, Object[] args)
        {
            return (T)assembly.CreateInstance(className, false, BindingFlags.CreateInstance, null, args, null, null);
        }

        private string OverrideAbstractMethods(Type type)
        {
            StringBuilder retVal = new StringBuilder();

            MethodInfo[] methods = type.GetMethods();
            foreach(MethodInfo method in methods)
            {
                if (method.IsAbstract && !method.IsSpecialName)
                {
                    retVal.Append(GenerateNonSpecialMethodOveride(method));
                }
            }

            return retVal.ToString();
        }

        private string GenerateNonSpecialMethodOveride(MethodInfo method)
        {
            StringBuilder retVal = new StringBuilder();

            retVal.Append(KEYWORD_PUBLIC + " " + KEYWORD_OVERRIDE + " " + GetReturnType(method.ReturnType.FullName) + " " + method.Name + " (");
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder parameterString = new StringBuilder();
            StringBuilder parameterTypeString = new StringBuilder();
            StringBuilder paramTypes = new StringBuilder();
            for (int j = 0; j < parameters.Length; j++)
            {
                ParameterInfo parameter = parameters[j];
                parameterTypeString.Append(parameter.ParameterType.FullName + " " + parameter.Name);
                paramTypes.Append(parameter.ParameterType.FullName);
                parameterString.Append(parameter.Name);
                if (j < parameters.Length - 1)
                {
                    parameterTypeString.Append(",");
                    parameterString.Append(",");
                    paramTypes.Append(",");
                }
            }
            _guid++;
            retVal.Append(parameterTypeString.ToString());
            retVal.Append(") {");
            if (!isVoid(method.ReturnType.FullName))
            {
                retVal.Append(KEYWORD_RETURN);
            }
            retVal.Append(" " + method.Name + "_" + _guid + "(" + parameterString.ToString() + ");");
            retVal.Append("}");

            retVal.Append(GetFunctionDelegateDetails(method.Name, parameterString.ToString(), paramTypes.ToString(), parameterTypeString.ToString(), method.ReturnType.FullName));

            return retVal.ToString();
        }

        private string GetFunctionDelegateDetails(string methodName, string parameterString, string paramTypes, string parameters, string returnType)
        {
            StringBuilder retVal = new StringBuilder();
            if(!isVoid(returnType))
            {
                if (paramTypes.Length > 0) paramTypes += ",";
                paramTypes += returnType;
            }
            
            //generate the setter
            retVal.Append(KEYWORD_PRIVATE + " System.Func<" + paramTypes + "> _" + methodName + ";"); 
            retVal.Append(KEYWORD_PUBLIC + " " + KEYWORD_VOID + " " + PREFIX_HIDDEN + methodName + "(System.Func<" + paramTypes + "> value) { _" + methodName + " = value; }");
            retVal.Append(KEYWORD_PUBLIC + " " + GetReturnType(returnType) + " " + methodName + "_" + _guid + "(" + parameters + ") {");
            if (!isVoid(returnType))
            {
                retVal.Append(KEYWORD_RETURN);
            }
            retVal.Append(" _" + methodName + "(" + parameterString + ");");
            retVal.Append("}"); 

            return retVal.ToString();
        }

        private string OverrideAbstractProperties(Type type)
        {
            StringBuilder retVal = new StringBuilder();

            PropertyInfo[] properties = type.GetProperties();
            foreach(PropertyInfo pi in properties)
            {
                if (isPropertyAbstract(pi))
                {
                    retVal.Append(KEYWORD_PRIVATE + " " + pi.PropertyType + " " + PREFIX_PRIVATE + pi.Name + ";");
                    Object[] obj = pi.GetCustomAttributes(true);
                    foreach(Attribute attr in obj)
                    {
                        //todo: add attribute params. So far I can't think of any way to reflect the
                        //called constructor to get the params.  So this is unsupported for now.
                        retVal.Append("[" + attr.GetType().FullName + "]");
                    }

                    retVal.Append(KEYWORD_PUBLIC + " " +
                        KEYWORD_OVERRIDE + " " +
                        pi.PropertyType + " " +
                        pi.Name + " { ");
                    if (pi.CanRead) retVal.Append("get { " + KEYWORD_RETURN + " " + PREFIX_PRIVATE + pi.Name + "; }");
                    if (pi.CanWrite) retVal.Append("set { " + PREFIX_PRIVATE + pi.Name + " = value; } ");
                    retVal.Append("}");

                    if (!pi.CanRead) retVal.Append(KEYWORD_PUBLIC + " " + pi.PropertyType + " " + PREFIX_HIDDEN + pi.Name + "{ get { " + KEYWORD_RETURN + " " + PREFIX_PRIVATE + pi.Name + "; } }");
                    if (!pi.CanWrite) retVal.Append(KEYWORD_PUBLIC + " " + pi.PropertyType + " " + PREFIX_HIDDEN + pi.Name + "{ set { " + PREFIX_PRIVATE + pi.Name + " = value; } }");

                }
            }

            return retVal.ToString();
        }

        private bool isPropertyAbstract(PropertyInfo property)
        {
            if (property.CanRead)
            {
                return property.GetGetMethod().IsAbstract;
            }
            else if(property.CanWrite)
            {
                return property.GetSetMethod().IsAbstract;
            }
            //shouldn't be possible
            return false;
        }

        private string GetReturnType(string value)
        {
            if (isVoid(value)) return KEYWORD_VOID;
            return value;
        }

        private bool isVoid(string value)
        {
            return value == "System.Void";
        }

        private string GetClassDefinition(Type type)
        {
            return KEYWORD_PUBLIC + " " + KEYWORD_SEALED + " " + KEYWORD_CLASS + " " + _classname + " : " + type.FullName + " {";
        }

        protected virtual string GetFQAN(Assembly currentAssembly)
        {
            return _namespace + "." + _classname;
        }

        private void GenerateClassName(Type type)
        {
            String[] splitClassName = type.FullName.Split(new char[1] { '.' });
            if (splitClassName.Length == 1)
            {
                _classname = splitClassName[0];
            }
            else
            {
                for (int i = 0; i < splitClassName.Length - 1; i++)
                {
                    _namespace += splitClassName[i];
                    if (i < splitClassName.Length - 2)
                    {
                        _namespace += ".";
                    }
                }
                _classname = splitClassName[splitClassName.Length - 1];
            }
        }
    }
}
