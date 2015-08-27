using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;
using System.IO;

namespace SharpWeld
{
    public class ObjectDecorator : Decorator 
    {
        private ObjectInstantiator _instantiator;
        private Object[] objects;

        public ObjectDecorator(ObjectInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public void PreInitialize(Type type)
        {
            objects = (Object[])type.GetCustomAttributes(typeof(ClassAttribute), true);
            foreach(Object obj in objects)
            {
                ClassAttribute classAttribute = (ClassAttribute)obj;
                classAttribute.BeforeInstantiation(type);
            }
        }

        public void PostInitialIze(Object instance)
        {
            foreach(ClassAttribute attr in objects)
            {
                ClassAttribute classAttribute = (ClassAttribute)attr;
                classAttribute.Decorator = this;
                classAttribute.First(instance);
                classAttribute.AfterInstantiation(instance);
                classAttribute.Last(instance);
            }
        }

        public T InitializeType<T>(Type type, Object [] args)  where T : class, new()
        {
            PreInitialize(type);
            T instance = _instantiator.GetInstanceByType<T>(type, args);
            PostInitialIze(instance);

            return instance;
        }

        public void Decorate<T>(T instance)
        {
            PropertyInfo[] properties = instance.GetType().GetProperties();
            foreach(PropertyInfo property in properties)
            {
                Object[] attributes = (Object[])property.GetCustomAttributes(typeof(Attribute), true);
                foreach(Object obj in attributes)
                {
                    PropertyAttribute propertyAttribute = _instantiator.GetInstanceByType<PropertyAttribute>(obj.GetType(), new Object[0]);
                    propertyAttribute.Decorator = this;
                    propertyAttribute.DecorateProperty(instance, property);
                }                
            }

            MethodInfo[] methods = instance.GetType().GetMethods();
            foreach(MethodInfo method in methods)
            {
                Object[] attributes = (Object[])method.GetCustomAttributes(typeof(MethodAttribute), true);
                for (int j = attributes.Length - 1; j >= 0; j--)
                {
                    Object obj = attributes[j];
                    MethodAttribute methodAttribute = _instantiator.GetInstanceByType <MethodAttribute>(obj.GetType(), new Object[0]);
                    methodAttribute.Decorator = this;
                    methodAttribute.DecorateMethod(instance, method);
                }
            }
        }

    }
}
