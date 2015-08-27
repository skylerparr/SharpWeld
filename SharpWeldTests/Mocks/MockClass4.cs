using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeldTest.Mocks
{
    public abstract class MockClass4
    {
        [MockMethodAbstract4("somevaluetoset")]
        public abstract string DoSomeMagic(string arg1, int arg2, string arg3, string arg4);

    }
}
