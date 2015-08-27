using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld.Attributes
{
    public abstract class AbstractClassAttribute : Attribute, AttributeDecorator
    {
        public virtual void First(Object obj) { }
        public virtual void Last(Object obj) { }
    }
}
