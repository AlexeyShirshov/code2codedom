using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToCodedom.Visitors;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Generator
{
    public static partial class Builder
    {
        public class NilClass { }

        public class CodeNilExpression : CodeExpression { };

        public static NilClass nil
        {
            get
            {
                return null;
            }
        }

        public static T Var<T>(string exp)
        {
            return default(T);
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

        public static CodeStatement[] GetStmts(params CodeStatement[] stmts)
        {
            return stmts;
        }

        public static T Eval<T>(Expression exp)
        {
            UnaryExpression ue = exp as UnaryExpression;
            if (ue != null)
            {
                return Eval<T>(ue.Operand);
            }
            else
            {
                LambdaExpression le = exp as LambdaExpression;
                if (le != null)
                {
                    return (T)le.Compile().DynamicInvoke();
                }
                else
                {
                    return (T)Expression.Lambda(exp).Compile().DynamicInvoke();
                }
            }
        }
    }
}
