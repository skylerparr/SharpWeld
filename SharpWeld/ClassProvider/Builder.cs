using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpWeld.ClassProvider
{
    public interface Builder
    {
        Assembly BuildAssembly(string code, Type type);
    }
}
