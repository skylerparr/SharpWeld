using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace SharpWeldTest.Mocks
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MockMethodAbstractAttribute : AbstractMethodHandlerAttribute
    {
        public MockMethodAbstractAttribute() { }

        public MockMethodAbstractAttribute(string value)
        {
        }

        protected override object GetHandler()
        {
            Func<string, int, string, string> function = DoSomeSortOfMagic;
            return function;
        }

        private string DoSomeSortOfMagic(string arg1, int arg2, string arg3)
        {
            return arg1 + arg2 + arg3;
        }

    }
}
