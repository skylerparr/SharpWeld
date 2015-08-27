using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpWeld.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true, Inherited=true)]
    public abstract class PropertyAttribute : InstanceAttribute
    {
        public abstract void DecorateProperty(Object obj, PropertyInfo propertyInfo);
    }
}
