using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeldTest.Mocks
{
    interface MockAttribute
    {
        int preInstantiated { get; set; }
        int postInstantiated { get; set; }

        int before { get; set; }
        int after { get; set; }
    }
}
