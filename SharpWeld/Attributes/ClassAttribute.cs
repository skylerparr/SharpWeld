using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld.Attributes
{
    public abstract class ClassAttribute : AbstractClassAttribute
    {
        public virtual void BeforeInstantiation(Type type) { }
        public virtual void AfterInstantiation(Object obj) { }
        public Decorator Decorator { protected get; set; }
    }
}
