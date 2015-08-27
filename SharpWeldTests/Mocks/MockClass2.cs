using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeldTest.Mocks
{
    public abstract class MockClass2
    {
        [MockMethodAbstract2]
        public abstract string OtherMagic(string arg1);
    }
}
