using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
    public class CodeVbXorExpression : CodeXorExpressionBase
	{
        public CodeVbXorExpression(CodeExpression left, CodeExpression right)
            : base(left, right)
		{
			
		}
		protected override void RefreshValue()
		{
			using (Microsoft.VisualBasic.VBCodeProvider provider = new Microsoft.VisualBasic.VBCodeProvider())
			{
				System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
				using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
				{
					tw.Write("(");
					provider.GenerateCodeFromExpression(Left, tw, opts);
					tw.Write(" Xor ");
					provider.GenerateCodeFromExpression(Right, tw, opts);
					tw.Write(")");
					Value = tw.InnerWriter.ToString();
				}
			}
		}
	}
}
