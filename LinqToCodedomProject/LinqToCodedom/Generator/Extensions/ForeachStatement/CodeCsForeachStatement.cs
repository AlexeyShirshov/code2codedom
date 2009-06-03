using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.IO;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeCsForeachStatement : CodeForeachStatementBase
    {

        public CodeCsForeachStatement()
        {

        }

		public CodeCsForeachStatement(CodeTypeReference iterationItemType, string iterationItemName,
			CodeExpression iterExpression, params CodeStatement[] statements)
			: base(iterationItemType, iterationItemName, iterExpression, statements)
        {
        }   
     

        protected override void RefreshValue()
        {
            using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
            {
                System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
                using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
                {
                    
                    tw.Write("foreach (");
                	provider.GenerateCodeFromExpression(new CodeTypeReferenceExpression(IterationItemType), tw, opts);
                	tw.Write(" {0} in ", provider.CreateValidIdentifier(IterationItemName));                	
                    provider.GenerateCodeFromExpression(IterExpression, tw, opts);
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
