using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace SharpWeldTest.Mocks
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MockMethodAbstract3Attribute : AbstractMethodHandlerAttribute
    {

        protected override object GetHandler()
        {
            Func<string, int, string> function = DoSomethingElse;
            return function;
        }

        private string DoSomethingElse(string arg1, int arg2)
        {
            return arg1 + arg2;
        }
    }
}
