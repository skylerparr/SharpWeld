using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;
using SharpWeld;

namespace SharpWeldTest.Mocks
{
    class MockMethod : MethodAttribute
    {
        public Object Obj { get; set; }
        public MethodInfo MethodInformation { get; set; }

        public override void DecorateMethod(Object obj, MethodInfo methodInfo)
        {
            ((MockObject2)obj).AttributeMethod = this;

            Obj = obj;
            MethodInformation = methodInfo;
        }

        public Decorator GetInjectedDecorator()
        {
            return Decorator;
        }
    }
}
