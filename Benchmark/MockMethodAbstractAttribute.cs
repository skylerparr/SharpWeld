using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace Benchmarking
{
    public class MockMethodAbstractAttribute : AbstractMethodHandlerAttribute
    {
        private string test;

        public MockMethodAbstractAttribute() { }

        public MockMethodAbstractAttribute(string value)
        {
            test = value;
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
