using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.CodeDom;
using LinqToCodedom.Visitors;

namespace LinqToCodedom.Generator
{
    public static partial class Builder
    {
        public static CodeAssignStatement assignVar<TResult, T>(string varName,
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(varName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignVar<TResult>(string varName,
            Expression<Func<TResult>> stmt)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(varName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assign<TResult, T>(Expression<Func<TResult, NilClass>> name,
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodeExpressionVisitor(new VisitorContext()).Visit(name),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }
    }
}
