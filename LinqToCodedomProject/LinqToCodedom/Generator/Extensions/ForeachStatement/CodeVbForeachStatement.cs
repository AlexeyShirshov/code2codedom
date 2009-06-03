using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeVbForeachStatement : CodeForeachStatementBase
    {

        public CodeVbForeachStatement()
        {

        }

        public CodeVbForeachStatement(CodeTypeReference iterationItemType, string iterationItemName,
			CodeExpression iterExpression, params CodeStatement[] statements)
			: base(iterationItemType, iterationItemName, iterExpression, statements)
        {
        }   


        protected override void RefreshValue()
        {
            using (Microsoft.VisualBasic.VBCodeProvider provider = new Microsoft.VisualBasic.VBCodeProvider())
            {
                System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
                using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
                {
                    tw.Write("For Each {0} As ", provider.CreateValidIdentifier(IterationItemName));
					provider.GenerateCodeFromExpression(new CodeTypeReferenceExpression(IterationItemType), tw, opts);
                    tw.Write(" in ");
                    provider.GenerateCodeFromExpression(IterExpression, tw, opts);
                    tw.WriteLine();
                    tw.Indent++;
                    if (Statements != null)
                        foreach (CodeStatement statement in Statements)
                        {
                            provider.GenerateCodeFromStatement(statement, tw, opts);
                        }
                    tw.Indent--;
					tw.WriteLine("Next");
                    Value = tw.InnerWriter.ToString();
                }
            }
        }
    }
}
