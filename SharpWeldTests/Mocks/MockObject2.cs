using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;
using System.Reflection;

namespace SharpWeldTest.Mocks
{
    class MockObject2
    {


        public MethodAttribute AttributeMethod { get; set; }

        [MockMethod]
        public void DecoratedMethod() 
        {
            
        }
    }
}
