using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
    public abstract class CodeForeachStatementBase : CodeSnippetStatement
    {
        private CodeTypeReference m_iterationItemType;
    	private string m_iterationItemName;
        private CodeExpression m_iterExpression;
        private CodeStatement[] m_statements;

    	protected CodeForeachStatementBase()
        {

        }

    	protected CodeForeachStatementBase(CodeTypeReference iterationItemType, string iterationItemName,
            CodeExpression iterExpression, params CodeStatement[] statements)
        {
            m_iterationItemType = iterationItemType;
			m_iterationItemName = iterationItemName;
            m_iterExpression = iterExpression;
            m_statements = statements;
            RefreshValue();
        }


        public CodeTypeReference IterationItemType
        {
			get { return m_iterationItemType; }
            set
            {
                m_iterationItemType = value;
                RefreshValue();
            }
        }

		public string IterationItemName
		{
			get { return m_iterationItemName; }
			set
			{
				m_iterationItemName = value;
				RefreshValue();
			}
		}

        public CodeExpression IterExpression
        {
            get { return m_iterExpression; }
            set
            {
                m_iterExpression = value;
                RefreshValue();
            }
        }

        protected abstract void RefreshValue();

        public CodeStatement[] Statements
        {
            get { return m_statements; }
            set
            {
                m_statements = value;
                RefreshValue();
            }
        }
    }
}
