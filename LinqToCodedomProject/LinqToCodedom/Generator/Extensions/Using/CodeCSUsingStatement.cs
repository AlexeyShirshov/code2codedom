using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeCSUsingStatement : CodeUsingStatementBase
    {

        public CodeCSUsingStatement()
        {

        }

        public CodeCSUsingStatement(CodeExpression usingExpression, params CodeStatement[] statements) : base(usingExpression, statements)
        {
        }   

        protected override void RefreshValue()
        {
            using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
            {
                System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
                using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
                {
                    
                    tw.Write("using (");
                    if(UsingExpression != null)
                        provider.GenerateCodeFromExpression(UsingExpression, tw, opts);
                    tw.WriteLine(")");
                    tw.WriteLine("{");
                    tw.Indent++;
                    if(Statements != null)
                        foreach (CodeStatement statement in Statements)
                        {
                            provider.GenerateCodeFromStatement(statement, tw, opts);
                        }
                    tw.Indent--;
                    tw.WriteLine("}");
                    Value = tw.InnerWriter.ToString();
                }
            }
        }
    }
}
