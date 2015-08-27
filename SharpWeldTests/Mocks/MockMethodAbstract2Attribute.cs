using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace SharpWeldTest.Mocks
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MockMethodAbstract2Attribute : AbstractMethodHandlerAttribute
    {

        protected override object GetHandler()
        {
            Func<string, string> function = DoSomethingElse;
            return function;
        }

        private string DoSomethingElse(string arg1)
        {
            return arg1;
        }
    }
}
