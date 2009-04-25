using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using System.Linq;

namespace LinqToCodedom
{
    public partial class CodeDom
    {
        private List<CodeNamespace> _namespaces = new List<CodeNamespace>();
        
        private System.Collections.Specialized.StringCollection _assemblies = 
            new System.Collections.Specialized.StringCollection() { "System.dll" };

        public enum Language { CSharp, VB };

        public CodeDom()
        {
        }

        public static CodeDomProvider CreateProvider(Language provider)
        {
            var providerOptions = new Dictionary<string, string>(); providerOptions.Add("CompilerVersion", "v3.5");

            switch (provider)
            {
                case Language.VB:
                    return new Microsoft.VisualBasic.VBCodeProvider(providerOptions);

                case Language.CSharp:
                default:
                    return new Microsoft.CSharp.CSharpCodeProvider(providerOptions);
            }
        }

        public CodeNamespace AddNamespace(string namespaceName)
        {
            CodeNamespace codeNamespace = new CodeNamespace(namespaceName);
            _namespaces.Add(codeNamespace);

            return codeNamespace;
        }

        public CodeDom AddReference(string referencedAssembly)
        {
            _assemblies.Add(referencedAssembly);

            return this;
        }

        public CodeCompileUnit CompileUnit
        {
            get
            {
                // Create a new CodeCompileUnit to contain 
                // the program graph.
                CodeCompileUnit compileUnit = new CodeCompileUnit();

                foreach (var ns in _namespaces)
                    compileUnit.Namespaces.Add(ns);

                return compileUnit;
            }
        }

        public Assembly Compile()
        {
            return Compile(null);
        }

        public Assembly Compile(string assemblyPath)
        {
            return Compile(assemblyPath, Language.CSharp);
        }

        public Assembly Compile(string assemblyPath, Language language)
        {
            CompilerParameters options = new CompilerParameters();
            options.IncludeDebugInformation = false;
            options.GenerateExecutable = false;
            options.GenerateInMemory = (assemblyPath == null);
            
            foreach (string refAsm in _assemblies)
                options.ReferencedAssemblies.Add(refAsm);
            
            if (assemblyPath != null)
                options.OutputAssembly = assemblyPath.Replace('\\', '/');

            using (CodeDomProvider codeProvider = CreateProvider(language))
            {
                CompilerResults results =
                   codeProvider.CompileAssemblyFromDom(options, CompileUnit);

                if (results.Errors.Count == 0)
                    return results.CompiledAssembly;

                // Process compilation errors
                Console.WriteLine("Compilation Errors:");
                
                foreach (string outpt in results.Output)
                    Console.WriteLine(outpt);

                foreach (CompilerError err in results.Errors)
                    Console.WriteLine(err.ToString());
            }

            return null;
        }

        public string GenerateCode(Language language)
        {
            StringBuilder sb = new StringBuilder();
            
            using (TextWriter tw = new IndentedTextWriter(new StringWriter(sb)))
            {
                using (CodeDomProvider codeProvider = CreateProvider(language))
                {
                    codeProvider.GenerateCodeFromCompileUnit(CompileUnit, tw, new CodeGeneratorOptions());
                }
            }

            return sb.ToString();
        }
    }
}
