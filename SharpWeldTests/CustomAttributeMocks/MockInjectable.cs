using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpWeld.CustomAttributes;

namespace SharpWeldTest.CustomAttributeMocks
{
    
    public abstract class MockInjectable
    {
        private Water water;

        [Inject]
        public Water Water {
            get
            {
                return water;
            }
            set
            {
                water = value;
            }
        }

        [Inject]
        public abstract Fire Fire { get; set; }

        [Inject]
        public abstract Earth Earth { get; }

        //[Inject("somewateryvalue")]
        //public abstract string MoreWater{get;set;}

    }
    
    public class Earth { }

    public class Fire { }

    public class Water { }
}
