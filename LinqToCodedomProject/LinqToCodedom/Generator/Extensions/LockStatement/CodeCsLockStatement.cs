using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeCsLockStatement : CodeLockStatementBase
    {

        public CodeCsLockStatement()
        {

        }

		public CodeCsLockStatement(CodeExpression lockExpression, params CodeStatement[] statements)
			: base(lockExpression, statements)
        {
        }   
     

        protected override void RefreshValue()
        {
            using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
            {
                System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
                using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
                {
                    
                    tw.Write("lock (");
                    if(LockExpression != null)
                        provider.GenerateCodeFromExpression(LockExpression, tw, opts);
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
