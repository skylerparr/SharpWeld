using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;

namespace SharpWeldTest.Mocks
{
    class MockClassAttribute2 : ClassAttribute, MockAttribute
    {
        public int preInstantiated { get; set; }
        public int postInstantiated { get; set; }

        public int before { get; set; }
        public int after { get; set; }

        private int order = 0;

        public override void BeforeInstantiation(Type type)
        {
            preInstantiated = order++;
        }

        public override void AfterInstantiation(Object obj)
        {
            postInstantiated = order++;
        }

        public override void First(object obj)
        {
            before = order++;
        }

        public override void Last(object obj)
        {
            after = order++;
            ((MockObjectMultipleAttributes)obj).addAttributeClass(this);
        }
    }
}
