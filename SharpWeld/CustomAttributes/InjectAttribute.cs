using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;

namespace SharpWeld.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true, Inherited=true)]
    public class InjectAttribute : PropertyAttribute
    {
        public InjectAttribute() { }

        private string type = "";

        public InjectAttribute(string type)
        {
            this.type = type; 
        }

        public override void DecorateProperty(object obj, PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite)
            {
                Type returnType = propertyInfo.GetSetMethod().GetParameters()[0].ParameterType;
                Object instance = Decorator.InitializeType<Object>(returnType, null);
                Decorator.Decorate<Object>(instance);
                if (type == "")
                {
                    propertyInfo.GetSetMethod().Invoke(obj, new Object[1] { instance });
                }
                else
                {
                    propertyInfo.GetSetMethod().Invoke(obj, new Object[1] { this.type });
                }
            }
            else if(propertyInfo.CanRead)
            {
                Type returnType = propertyInfo.GetGetMethod().ReturnType;
                Object instance = Decorator.InitializeType<Object>(returnType, null);
                Decorator.Decorate<Object>(instance);
                if (type == "")
                {
                    obj.GetType().GetMethod("set___" + propertyInfo.Name).Invoke(obj, new Object[1]{instance});
                }
                else
                {
                    obj.GetType().GetProperty("set___" + propertyInfo.Name).GetSetMethod().Invoke(obj, new Object[1] { this.type });
                }
            }
        }
    }
}
