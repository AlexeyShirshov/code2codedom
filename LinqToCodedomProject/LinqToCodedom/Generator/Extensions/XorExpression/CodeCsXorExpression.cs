using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinqToCodedom.CodeDomPatterns
{
	public class CodeCsXorExpression : CodeXorExpressionBase
	{
        public CodeCsXorExpression(CodeExpression left, CodeExpression right)
            : base(left, right)
		{
			
		}
		 
		protected override void RefreshValue()
		{
			using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
			{
				System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
				using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
				{
					tw.Write("(");
					provider.GenerateCodeFromExpression(Left, tw, opts);
					tw.Write(" ^ ");
					provider.GenerateCodeFromExpression(Right, tw, opts);
					tw.Write(")");
					Value = tw.InnerWriter.ToString();
				}
			}
		}
	}
}
