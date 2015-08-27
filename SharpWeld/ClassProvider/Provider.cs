using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace SharpWeld.ClassProvider
{
    public interface Provider
    {
        CompilerResults CompileAssemblyFromSource(CompilerParameters cp, string[] code);
    }
}
