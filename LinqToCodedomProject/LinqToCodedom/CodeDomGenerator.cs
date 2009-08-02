﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System;
using System.Linq;
using LinqToCodedom.Extensions;
using System.Collections;
using LinqToCodedom.CustomCodeDomGeneration;
using LinqToCodedom.CodeDomPatterns;

namespace LinqToCodedom
{
    public class CodeDomGenerator
    {
        private List<CodeNamespace> _namespaces = new List<CodeNamespace>();

        private List<string> _assemblies = new List<string> { "System.dll" };

        public enum Language { CSharp, VB };

        public CodeDomGenerator()
        {
        }

        public static CodeDomProvider CreateProvider(Language provider)
        {
            var providerOptions = new Dictionary<string, string>(); providerOptions.Add("CompilerVersion", "v3.5");

            switch (provider)
            {
                case Language.VB:
                    return new CustomCodeDomGeneration.VBCustomCodeProvider(providerOptions);
                case Language.CSharp:
                    return new CustomCodeDomGeneration.CSCustomCodeProvider(providerOptions);
                default:
                    throw new NotImplementedException(provider.ToString());
            }
        }

        public CodeNamespace AddNamespace(string namespaceName)
        {
            CodeNamespace codeNamespace = new CodeNamespace(namespaceName);
            _namespaces.Add(codeNamespace);

            return codeNamespace;
        }

        public CodeNamespace AddNamespace(CodeNamespace codeNamespace)
        {
            _namespaces.Add(codeNamespace);

            return codeNamespace;
        }

        public CodeDomGenerator AddReference(string referencedAssembly)
        {
            _assemblies.Add(referencedAssembly);

            return this;
        }

        public CodeCompileUnit GetCompileUnit(Language language)
        {
            // Create a new CodeCompileUnit to contain 
            // the program graph.
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeDomTreeProcessor.ProcessNS(compileUnit, language, _namespaces);

            return compileUnit;
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
                   codeProvider.CompileAssemblyFromDom(options, GetCompileUnit(language));

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

        public static Assembly Compile(string assemblyPath, Language language, 
            IEnumerable<string> assemblies,
            params CodeCompileUnit[] units)
        {
            CompilerParameters options = new CompilerParameters();
            options.IncludeDebugInformation = false;
            options.GenerateExecutable = false;
            options.GenerateInMemory = (assemblyPath == null);

            foreach (string refAsm in assemblies)
                options.ReferencedAssemblies.Add(refAsm);

            if (assemblyPath != null)
                options.OutputAssembly = assemblyPath.Replace('\\', '/');

            using (CodeDomProvider codeProvider = CreateProvider(language))
            {
                CompilerResults results =
                   codeProvider.CompileAssemblyFromDom(options, units);

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
            return GenerateCode(language, GetCompileUnit(language));
        }

        public static string GenerateCode(Language language, CodeCompileUnit unit)
        {
            StringBuilder sb = new StringBuilder();

            GenerateCode(new StringWriter(sb), language, unit);

            return sb.ToString();
        }

        public static void GenerateCode(TextWriter wr, Language language, CodeCompileUnit unit)
        {
            using (TextWriter tw = new IndentedTextWriter(wr))
            {
                using (CodeDomProvider codeProvider = CreateProvider(language))
                {
                    codeProvider.GenerateCodeFromCompileUnit(unit, tw, new CodeGeneratorOptions());
                }
            }
        }

        public IEnumerable<CodeNamespace> Namespaces
        {
            get
            {
                return _namespaces;
            }
        }
    }
}
