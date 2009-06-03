using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinqToCodedom.CodeDomPatterns
{
	public class CodeVbMemberOperatorOverride : CodeMemberOperatorOverride
	{

		#region Overrides of CodeMemberOperatorOverride

		protected override void GenerateOperator()
		{
			IMemberOperatorGenerator gen = null;
			switch (Operator)
			{
				case OperatorType.Explicit:
					gen = new ExplicitOperatorGenerator();
					break;
				case OperatorType.Implicit:
					gen = new ImplicitOperatorGenerator();
					break;
				default:
					throw new NotImplementedException(string.Format("Какая-то несанкционированная xуйня"));
			}

			using (Microsoft.VisualBasic.VBCodeProvider provider = new Microsoft.VisualBasic.VBCodeProvider())
			{
				System.CodeDom.Compiler.CodeGeneratorOptions opts = new CodeGeneratorOptions();
				using (System.CodeDom.Compiler.IndentedTextWriter tw = new IndentedTextWriter(new StringWriter(), opts.IndentString))
				{
					gen.GenerateDeclaration(tw, provider, opts, ReturnType, Parameters);
					tw.Indent++;
					gen.GenerateStatemets(tw, provider, opts, Statements);
					tw.Indent--;
					tw.WriteLine("End Operator");
					Text = tw.InnerWriter.ToString();
				}
			}
		}

		#endregion

		protected abstract class ConversionOperatorGeneratorBase : CodeMemberOperatorOverride.IMemberOperatorGenerator
		{
			#region Implementation of IMemberOperatorGenerator

			protected abstract string OperatorName
			{
				get;
			}

			public void GenerateDeclaration(TextWriter tw, CodeDomProvider provider, CodeGeneratorOptions opts, CodeTypeReference returnType, CodeParameterDeclarationExpression[] parameters)
			{
				tw.Write(string.Format("Public Shared {0} Operator CType", OperatorName));
				tw.Write("(");
				for (int i = 0; i < parameters.Length; i++)
				{
					var parameter = parameters[i];
					provider.GenerateCodeFromExpression(parameter, tw, opts);
					if (i != (parameters.Length - 1))
						tw.Write(", ");
				}

				tw.Write(") As ");
				provider.GenerateCodeFromExpression(new CodeTypeReferenceExpression(returnType), tw, opts);
				tw.WriteLine();
			}

			public void GenerateStatemets(TextWriter tw, CodeDomProvider provider, CodeGeneratorOptions opts, CodeStatement[] statements)
			{
				foreach (var statement in statements)
				{
					provider.GenerateCodeFromStatement(statement, tw, opts);
				}
			}

			#endregion
		}

		protected class ExplicitOperatorGenerator : ConversionOperatorGeneratorBase
		{
			private const string c_name = "Narrowing";

			#region Overrides of ConversionOperatorGeneratorBase

			protected override string OperatorName
			{
				get
				{
					return c_name;
				}
			}

			#endregion
		}

		protected class ImplicitOperatorGenerator : ConversionOperatorGeneratorBase
		{
			private const string c_name = "Widening";

			#region Overrides of ConversionOperatorGeneratorBase

			protected override string OperatorName
			{
				get
				{
					return c_name;
				}
			}

			#endregion
		}
	}
}
