using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;

namespace SharpWeld.CustomAttributes
{
	public class InjectAttribute : PropertyAttribute
    {
        public InjectAttribute() { }

        public override void DecorateProperty(object obj, PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite)
            {
                Type returnType = propertyInfo.GetSetMethod().GetParameters()[0].ParameterType;
                Object instance = Decorator.InitializeType<Object>(returnType, null);
                Decorator.Decorate<Object>(instance);
                propertyInfo.GetSetMethod().Invoke(obj, new Object[1] { instance });
            }
            else if(propertyInfo.CanRead)
            {
                Type returnType = propertyInfo.GetGetMethod().ReturnType;
                Object instance = Decorator.InitializeType<Object>(returnType, null);
                Decorator.Decorate<Object>(instance);
                obj.GetType().GetMethod("set___" + propertyInfo.Name).Invoke(obj, new Object[1]{instance});
            }
        }
    }
}
