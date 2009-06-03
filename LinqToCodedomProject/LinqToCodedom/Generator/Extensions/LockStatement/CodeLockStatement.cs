using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
    public abstract class CodeLockStatementBase : CodeSnippetStatement
    {
        private CodeExpression m_lockExpression;
        private CodeStatement[] m_statements;

        public CodeLockStatementBase()
        {

        }

		public CodeLockStatementBase(CodeExpression lockExpression, params CodeStatement[] statements)
        {
			m_lockExpression = lockExpression;
            m_statements = statements;
            RefreshValue();
        }


        public CodeExpression LockExpression
        {
			get { return m_lockExpression; }
            set
            {
				m_lockExpression = value;
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
