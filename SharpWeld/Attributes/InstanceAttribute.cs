using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpWeld.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple=true, Inherited=true)]
    public abstract class InstanceAttribute : Attribute
    {
        public Decorator Decorator { protected get; set; }
    }
}
