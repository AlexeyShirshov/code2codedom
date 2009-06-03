using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom;

namespace LinqToCodedom.CodeDomPatterns
{
    public abstract class CodeUsingStatementBase : CodeSnippetStatement
    {
        private CodeExpression m_usingExpression;
        private CodeStatement[] m_statements;

        public CodeUsingStatementBase()
        {

        }

        public CodeUsingStatementBase(CodeExpression usingExpression, params CodeStatement[] statements)
        {
            m_usingExpression = usingExpression;
            m_statements = statements;
            RefreshValue();
        }


        public CodeExpression UsingExpression
        {
            get { return m_usingExpression; }
            set
            {
                m_usingExpression = value;
                RefreshValue();
            }
        }

        protected abstract void RefreshValue();

        public CodeStatement[] Statements
        {
            get
            {
            	return m_statements;
            }
            set
            {
                m_statements = value;
                RefreshValue();
            }
        }
    }
}
