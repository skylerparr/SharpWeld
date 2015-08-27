using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;
using SharpWeld;

namespace SharpWeldTest.Mocks
{
    class MockPropertyAttribute : PropertyAttribute
    {
        public Object Obj { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public override void DecorateProperty(object obj, PropertyInfo propertyInfo)
        {
            ((MockObject)obj).AttributeProperty = this;

            Obj = obj;
            PropertyInfo = propertyInfo;
        }

        public Decorator GetInjectedDecorator()
        {
            return Decorator;
        }
    }
}
