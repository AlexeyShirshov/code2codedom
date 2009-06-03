using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
	public abstract class CodeXorExpressionBase : CodeSnippetExpression
	{
		private CodeExpression m_left;
		private CodeExpression m_right;

        public CodeXorExpressionBase(CodeExpression left, CodeExpression right)
		{
			m_left = left;
			m_right = right;
			RefreshValue();
			
		}

        public CodeExpression Left
		{
			get { return m_left; }
			set 
			{
				if (m_left != value)
				{
					m_left = value;
					RefreshValue();
				}
			}
		}

		public CodeExpression Right
		{
			get { return m_right; }
			set
			{
				if (m_right != value)
				{
					m_right = value;
					RefreshValue();
				}
			}
		}

		protected abstract void RefreshValue();
	}
}
