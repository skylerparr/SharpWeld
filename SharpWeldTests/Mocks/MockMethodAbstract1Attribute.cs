using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace SharpWeldTest.Mocks
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MockMethodAbstract1Attribute : AbstractMethodHandlerAttribute
    {
        public MockMethodAbstract1Attribute() { }

        public MockMethodAbstract1Attribute(string value)
        {
        }

        protected override object GetHandler()
        {
            Func<string> function = DoSomethingElse;
            return function;
        }

        private string DoSomethingElse()
        {
            return "This is a sentence";
        }

    }
}
