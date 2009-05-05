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

        public static CodeAssignStatement assignField<TResult, T>(CodeExpression target, string fieldName,
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodeFieldReferenceExpression(target, fieldName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignField<TResult>(CodeExpression target, string fieldName,
            Expression<Func<TResult>> stmt)
        {
            return new CodeAssignStatement(
                new CodeFieldReferenceExpression(target, fieldName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignField<TResult, T>(string fieldName,
           Expression<Func<TResult, T>> stmt)
        {
            return assignField<TResult, T>(new CodeThisReferenceExpression(), fieldName, stmt);
        }

        public static CodeAssignStatement assignField<TResult>(string fieldName,
            Expression<Func<TResult>> stmt)
        {
            return assignField<TResult>(new CodeThisReferenceExpression(), fieldName, stmt);
        }

        public static CodeAssignStatement assignProperty<TResult, T>(CodeExpression target, string propertyName,
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodePropertyReferenceExpression(target, propertyName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignProperty<TResult>(CodeExpression target, string propertyName,
            Expression<Func<TResult>> stmt)
        {
            return new CodeAssignStatement(
                new CodePropertyReferenceExpression(target, propertyName),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignProperty<TResult, T>(string propertyName,
           Expression<Func<TResult, T>> stmt)
        {
            return assignProperty<TResult, T>(new CodeThisReferenceExpression(), propertyName, stmt);
        }

        public static CodeAssignStatement assignProperty<TResult>(string propertyName,
            Expression<Func<TResult>> stmt)
        {
            return assignProperty<TResult>(new CodeThisReferenceExpression(), propertyName, stmt);
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
