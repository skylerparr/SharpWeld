using System;
using System.Collections;
using System.Linq;
using System.Text;
using SharpWeld.Attributes;

namespace SharpWeldTest.Mocks
{
    [MockClassAttribute]
    [MockClassAttribute2]
    [MockClassAtrribute3]
    public class MockObjectMultipleAttributes
    {
        private ArrayList attributes = new ArrayList();

        public ArrayList AttibuteClasses
        {
            get
            {
                return attributes;
            }
            set
            {

            }
        }

        public void addAttributeClass(ClassAttribute attr)
        {
            attributes.Add(attr);
        }
    }
}
