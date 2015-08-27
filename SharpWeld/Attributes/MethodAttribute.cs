using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpWeld.Attributes
{
    public abstract class MethodAttribute : InstanceAttribute
    {
        public abstract void DecorateMethod(Object obj, MethodInfo methodInfo);
    }
}
