using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection.Emit;

namespace SharpWeld.ClassProvider
{
    public class AssemblyBuilder : Builder
    {
        private Provider provider;

        public AssemblyBuilder(Provider value) 
        {
            this.provider = value;
        }

        public Assembly BuildAssembly(string code, Type type)
        {
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;
            cp.IncludeDebugInformation = true;
            cp.CompilerOptions = "/optimize";
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            cp.ReferencedAssemblies.Add("System.dll");
            if (type != null)
            {
                Assembly asm = Assembly.GetAssembly(type);
                cp.ReferencedAssemblies.Add(asm.Location);
            }
            CompilerResults results = provider.CompileAssemblyFromSource(cp, new string[1] { code });

            return results.CompiledAssembly;
        }
    }
}
