using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToCodedom.Visitors;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Generator
{
    public static class Builder
    {
        public static CodeStatement stmt(Expression<Sub> exp)
        {
            return new CodeStatementVisitor().Visit(exp);
        }

        public static CodeStatement stmt<T>(Expression<Sub<T>> exp)
        {
            return new CodeStatementVisitor().Visit(exp);
        }

        public static CodeStatement Line<TResult, T>(Expression<Func<TResult, T>> exp)
        {
            return null;
        }

        private static string GetMethodName(LambdaExpression exp, CodeParameterDeclarationExpressionCollection pars)
        {
            foreach (var p in exp.Parameters)
            {
                var par = new CodeParameterDeclarationExpression(p.Type, p.Name);
                pars.Add(par);
            }
            return Eval<string>(exp.Body);
        }

        public static CodeMemberMethod Method<TResult, T>(Type returnType, MemberAttributes ma, 
            Expression<Func<TResult, T>> paramsAndName, params CodeStatement[] statements)
        {
            CodeMemberMethod method = new CodeMemberMethod()
            {
                ReturnType = returnType == null ? null : new CodeTypeReference(returnType),
                Attributes = ma
            };

            method.Name = GetMethodName(paramsAndName, method.Parameters);
            method.Statements.AddRange(statements);

            return method;
        }

        public static T Eval<T>(Expression exp)
        {
            return (T)Expression.Lambda(exp).Compile().DynamicInvoke();
        }
    }
}
