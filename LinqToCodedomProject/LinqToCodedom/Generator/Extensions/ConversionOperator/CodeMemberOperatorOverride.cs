using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
	public enum OperatorType
	{
		Explicit,
		Implicit
	}

	

	public abstract class CodeMemberOperatorOverride : CodeSnippetTypeMember
	{
		protected interface IMemberOperatorGenerator
		{
			void GenerateDeclaration(TextWriter tw, CodeDomProvider provider, CodeGeneratorOptions opts, CodeTypeReference returnType, CodeParameterDeclarationExpression[] parameters);
			void GenerateStatemets(TextWriter tw, CodeDomProvider provider, CodeGeneratorOptions opts, CodeStatement[] statements);
		}

		private OperatorType m_operator;
		private CodeParameterDeclarationExpression[] m_parameters;
		private CodeStatement[] m_statements;
		private CodeTypeReference m_returnType;

		protected void RefreshBody()
		{
			if (m_parameters == null || m_statements == null || m_returnType == null)
				return;
			GenerateOperator();
		}

		protected abstract void GenerateOperator();

		public OperatorType Operator
		{
			get
			{
				return m_operator;
			}
			set
			{
				m_operator = value;
				RefreshBody();
			}
		}

		public CodeTypeReference ReturnType
		{
			get
			{
				return m_returnType;
			}
			set
			{
				m_returnType = value;
				RefreshBody();
			}
		}
		public CodeParameterDeclarationExpression[] Parameters
		{
			get
			{
				return m_parameters;
			}
			set
			{
				m_parameters = value;
				RefreshBody();
			}
		}

		public CodeStatement[] Statements
		{
			get
			{
				return m_statements;
			}
			set
			{
				m_statements = value;
				RefreshBody();
			}
		}
	}
}
