using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace SharpWeld.ClassProvider
{
    public class CSharpProvider : Provider
    {
        public CompilerResults CompileAssemblyFromSource(CompilerParameters cp, string[] code)
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
            return provider.CompileAssemblyFromSource(cp, code);
        }
    }
}
