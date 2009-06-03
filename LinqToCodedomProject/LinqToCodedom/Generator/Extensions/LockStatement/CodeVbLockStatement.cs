using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeVbLockStatement : CodeLockStatementBase
    {

        public CodeVbLockStatement()
        {

        }

        public CodeVbLockStatement(CodeExpression lockExpression, params CodeStatement[] statements)
            : base(lockExpression, statements)
        {
        }


        protected override void RefreshValue()
        {
            using (Microsoft.VisualBasic.VBCodeProvider provider = new Microsoft.VisualBasic.VBCodeProvider())
            {
                System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
                using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
                {
                    tw.Write("SyncLock ");
                    if (LockExpression != null)
                        provider.GenerateCodeFromExpression(LockExpression, tw, opts);
                    tw.WriteLine();
                    tw.Indent++;
                    if (Statements != null)
                        foreach (CodeStatement statement in Statements)
                        {
                            provider.GenerateCodeFromStatement(statement, tw, opts);
                        }
                    tw.Indent--;
					tw.WriteLine("End SyncLock");
                    Value = tw.InnerWriter.ToString();
                }
            }
        }
    }
}
