using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
	public class CodeCsMemberOperatorOverride : CodeMemberOperatorOverride
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

			using (Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider())
			{
				System.CodeDom.Compiler.CodeGeneratorOptions opts = new System.CodeDom.Compiler.CodeGeneratorOptions();
				using (System.CodeDom.Compiler.IndentedTextWriter tw = new System.CodeDom.Compiler.IndentedTextWriter(new StringWriter(), opts.IndentString))
				{
					gen.GenerateDeclaration(tw, provider, opts, ReturnType, Parameters);
					tw.WriteLine("{");
					tw.Indent++;
					gen.GenerateStatemets(tw, provider, opts, Statements);
					tw.Indent--;
					tw.WriteLine("}");
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
				tw.Write(string.Format("public static {0} operator ", OperatorName));
				provider.GenerateCodeFromExpression(new CodeTypeReferenceExpression(returnType), tw, opts);
				tw.Write("(");
				for (int i = 0; i < parameters.Length; i++)
				{
					var parameter = parameters[i];
					provider.GenerateCodeFromExpression(parameter, tw, opts);
					if (i != (parameters.Length - 1))
						tw.Write(", ");
				}
				
				tw.WriteLine(")");
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
			private const string c_name = "explicit";

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
			private const string c_name = "implicit";

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
