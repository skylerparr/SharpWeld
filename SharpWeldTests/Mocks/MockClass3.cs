using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeldTest.Mocks
{
    public abstract class MockClass3
    {
        [MockMethodAbstract3]
        public abstract string OtherMagic(string arg1, int arg2);
    }
}
