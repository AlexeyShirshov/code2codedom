using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Visitors;
using System.Linq.Expressions;

namespace LinqToCodedom.Generator
{
    public static partial class Builder
    {
        public static CodeConditionStatement @if<T>(Expression<Func<T, bool>> condition,
            params CodeStatement[] trueStatements)
        {
            return ifelse(condition, trueStatements, null);
        }

        public static CodeConditionStatement ifelse<T>(Expression<Func<T, bool>> condition,
            CodeStatement[] trueStatements, params CodeStatement[] falseStatements)
        {
            var condStatement = new CodeConditionStatement();
            condStatement.Condition = new CodeExpressionVisitor(new VisitorContext()).Visit(condition);
            condStatement.TrueStatements.AddRange(trueStatements);
            if (falseStatements != null)
                condStatement.FalseStatements.AddRange(falseStatements);
            return condStatement;
        }

    }
}
