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
        public class NilClass { }

        public class CodeNilExpression : CodeExpression { };

        public static NilClass nil
        {
            get
            {
                return null;
            }
        }

        public static CodeStatement stmt(Expression<Action> exp)
        {
            return new CodeStatementVisitor(new VisitorContext()).Visit(exp);
        }

        public static CodeStatement stmt<T>(Expression<Action<T>> exp)
        {
            return new CodeStatementVisitor(new VisitorContext()).Visit(exp);
        }

        public static CodeStatement stmt<TResult>(Expression<Func<TResult>> exp)
        {
            return new CodeStatementVisitor(new VisitorContext()).Visit(exp);
        }

        public static CodeStatement stmt<TResult, T>(Expression<Func<TResult, T>> exp)
        {
            return new CodeStatementVisitor(new VisitorContext()).Visit(exp);
        }

        public static CodeStatement @return<TResult>(Expression<Func<TResult>> exp)
        {
            return new CodeMethodReturnStatement(new CodeExpressionVisitor(new VisitorContext()).Visit(exp));
        }

        public static CodeStatement @return<TResult, T>(Expression<Func<TResult, T>> exp)
        {
            return new CodeMethodReturnStatement(new CodeExpressionVisitor(new VisitorContext()).Visit(exp));
        }

        public static CodeStatement @return<TResult>(Expression<Action<TResult>> exp)
        {
            return new CodeMethodReturnStatement(new CodeExpressionVisitor(new VisitorContext()).Visit(exp));
        }

        public static T Var<T>(Expression<Func<string>> exp)
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

        public static CodeAssignStatement assignVar<TResult, T>(Expression<Func<string>> varName, 
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(varName.Compile().Invoke()),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assignVar<TResult>(Expression<Func<string>> varName,
            Expression<Func<TResult>> stmt)
        {
            return new CodeAssignStatement(
                new CodeVariableReferenceExpression(varName.Compile().Invoke()),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeAssignStatement assign<TResult, T>(Expression<Func<TResult, NilClass>> name,
            Expression<Func<TResult, T>> stmt)
        {
            return new CodeAssignStatement(
                new CodeExpressionVisitor(new VisitorContext()).Visit(name),
                new CodeExpressionVisitor(new VisitorContext()).Visit(stmt));
        }

        public static CodeIterationStatement @for<TResult, T, T2, T3>(Expression<Func<string>> varName,
            Expression<Func<T, TResult>> initStmt,
            Expression<Func<T2, bool>> testStmt, Expression<Func<T3, TResult>> incStmt,
            params CodeStatement[] statements)
        {
            return new CodeIterationStatement(declare(varName, initStmt),
                new CodeExpressionVisitor(new VisitorContext()).Visit(testStmt),
                Builder.assignVar(varName,incStmt),
                statements);
        }

        public static CodeIterationStatement @for<TResult, T, T2>(Expression<Func<string>> varName,
            Expression<Func<T, TResult>> initStmt,
            Expression<Func<T2, bool>> testStmt, Expression<Func<TResult>> incStmt,
            params CodeStatement[] statements)
        {
            return new CodeIterationStatement(declare(varName, initStmt),
                new CodeExpressionVisitor(new VisitorContext()).Visit(testStmt),
                Builder.assignVar(varName, incStmt),
                statements);
        }

        public static CodeIterationStatement @for<TResult, T, T2, T3>(Expression<Func<string>> varName,
            Expression<Func<T, T3, TResult>> initStmt,
            Expression<Func<T2, bool>> testStmt, Expression<Func<TResult>> incStmt,
            params CodeStatement[] statements)
        {
            return new CodeIterationStatement(
                new CodeStatementVisitor(new VisitorContext()).Visit(initStmt),
                new CodeExpressionVisitor(new VisitorContext()).Visit(testStmt),
                new CodeStatementVisitor(new VisitorContext()).Visit(incStmt),
                statements);
        }

        public static CodeVariableDeclarationStatement declare<TResult>(
            Expression<Func<string>> varName, Expression<Func<TResult>> initExp)
        {
            return new CodeVariableDeclarationStatement(typeof(TResult), varName.Compile().Invoke(),
                new CodeExpressionVisitor(new VisitorContext()).Visit(initExp));
        }

        public static CodeVariableDeclarationStatement declare<T, TResult>(
            Expression<Func<string>> varName, Expression<Func<T, TResult>> initExp)
        {
            return new CodeVariableDeclarationStatement(typeof(TResult), varName.Compile().Invoke(),
                new CodeExpressionVisitor(new VisitorContext()).Visit(initExp));
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

        public static CodeConditionStatement @if<T>(Expression<Func<T, bool>> condition,
            params CodeStatement[] trueStatements)
        {
            return @if(condition, trueStatements, null);
        }

        public static CodeConditionStatement @if<T>(Expression<Func<T, bool>> condition,
            CodeStatement[] trueStatements, params CodeStatement[] falseStatements)
        {
            var condStatement = new CodeConditionStatement();
            condStatement.Condition = new CodeExpressionVisitor(new VisitorContext()).Visit(condition);
            condStatement.TrueStatements.AddRange(trueStatements);
            if (falseStatements != null)
                condStatement.FalseStatements.AddRange(falseStatements);
            return condStatement;
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
