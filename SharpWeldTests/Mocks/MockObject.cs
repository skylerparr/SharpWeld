using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;

namespace SharpWeldTest.Mocks
{
    [MockClassAttribute]
    public class MockObject
    {
        public ClassAttribute AttibuteClass { get; set; }

        public PropertyAttribute AttributeProperty{ get; set;}


        [MockProperty]
        public string DecoratedString { get; set; }

        public string Arg1 { get; set; }
        public string Arg2 { get; set; }

        public MockObject() { }

        public MockObject(string arg1, string arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }

    }
}
