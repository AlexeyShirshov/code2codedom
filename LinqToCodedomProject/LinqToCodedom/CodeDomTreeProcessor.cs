using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.CustomCodeDomGeneration;
using LinqToCodedom.CodeDomPatterns;
using System.Collections;
using LinqToCodedom.Extensions;

namespace LinqToCodedom
{
    public static class CodeDomTreeProcessor
    {
        class Pair<T, T2>
        {
            public T First;
            public T2 Second;
            public Pair(T first, T2 second)
            {
                First = first;
                Second = second;
            }
        }

        public static void ProcessNS(CodeCompileUnit compileUnit, LinqToCodedom.CodeDomGenerator.Language language, IEnumerable<CodeNamespace> namespaces)
        {
            foreach (CodeNamespace ns in namespaces)
            {
                CodeNamespace ns2add = ns;
                for (int j = 0; j < ns.Types.Count; j++)
                {
                    CodeTypeDeclaration c = ns.Types[j];
                    List<Pair<int, CodeTypeMember>> toReplace = new List<Pair<int, CodeTypeMember>>();
                    for (int i = 0; i < c.Members.Count; i++)
                    {
                        CodeTypeMember m = c.Members[i];
                        CodeTypeMember newMember = ProcessMember(m, language);
                        if (newMember != m)
                            toReplace.Add(new Pair<int, CodeTypeMember>(i, newMember));
                    }
                    if (toReplace.Count > 0)
                    {
                        if (ns2add == ns)
                            ns2add = ns.Clone() as CodeNamespace;

                        c = ns2add.Types[j];
                        foreach (Pair<int, CodeTypeMember> p in toReplace)
                        {
                            int idx = p.First;
                            c.Members.RemoveAt(idx);
                            c.Members.Insert(idx, p.Second);
                        }
                    }
                }
                compileUnit.Namespaces.Add(ns2add);
            }
        }

        private static CodeTypeMember ProcessMember(CodeTypeMember m, LinqToCodedom.CodeDomGenerator.Language language)
        {
            if (typeof(CodeMemberMethod).IsAssignableFrom(m.GetType()))
                foreach (CodeStatement stmt in ((CodeMemberMethod)m).Statements)
                {
                    ProcessStmt(stmt, language);
                }

            if (typeof(CodeMemberProperty).IsAssignableFrom(m.GetType()))
            {
                foreach (CodeStatement stmt in ((CodeMemberProperty)m).GetStatements)
                {
                    ProcessStmt(stmt, language);
                }

                foreach (CodeStatement stmt in ((CodeMemberProperty)m).SetStatements)
                {
                    ProcessStmt(stmt, language);
                }
            }

            if (typeof(CodeConstructor).IsAssignableFrom(m.GetType()))
            {
                ProcessExpr(((CodeConstructor)m).BaseConstructorArgs, language);
                ProcessExpr(((CodeConstructor)m).ChainedConstructorArgs, language);
                ProcessStmt(((CodeConstructor)m).Statements, language);
            }

            if (typeof(CodeMemberField).IsAssignableFrom(m.GetType()))
            {
                ProcessExpr(((CodeMemberField)m).InitExpression, language);
            }

            ICustomCodeDomObject co = m as ICustomCodeDomObject;
            if (co != null)
                co.GenerateCode(language);

            if (typeof(CodeMemberMethod).IsAssignableFrom(m.GetType()))
                return ProcessMethod(m as CodeMemberMethod, language);

            return m;
        }

        private static void ProcessStmt(CodeStatement stmt, LinqToCodedom.CodeDomGenerator.Language language)
        {
            if (stmt == null) return;

            if (typeof(CodeAssignStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeAssignStatement)stmt).Left, language);
                ProcessExpr(((CodeAssignStatement)stmt).Right, language);
            }
            else if (typeof(CodeAttachEventStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeAttachEventStatement)stmt).Listener, language);
            }
            else if (typeof(CodeCommentStatement).IsAssignableFrom(stmt.GetType()))
            {

            }
            else if (typeof(CodeConditionStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeConditionStatement)stmt).Condition, language);
                ProcessStmt(((CodeConditionStatement)stmt).TrueStatements, language);
                ProcessStmt(((CodeConditionStatement)stmt).FalseStatements, language);
            }
            else if (typeof(CodeExpressionStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeExpressionStatement)stmt).Expression, language);
            }
            else if (typeof(CodeGotoStatement).IsAssignableFrom(stmt.GetType()))
            {
            }
            else if (typeof(CodeIterationStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessStmt(((CodeIterationStatement)stmt).IncrementStatement, language);
                ProcessStmt(((CodeIterationStatement)stmt).InitStatement, language);
                ProcessStmt(((CodeIterationStatement)stmt).Statements, language);
                ProcessExpr(((CodeIterationStatement)stmt).TestExpression, language);
            }
            else if (typeof(CodeLabeledStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessStmt(((CodeLabeledStatement)stmt).Statement, language);
            }
            else if (typeof(CodeMethodReturnStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeMethodReturnStatement)stmt).Expression, language);
            }
            else if (typeof(CodeRemoveEventStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeRemoveEventStatement)stmt).Listener, language);
            }
            else if (typeof(CodeThrowExceptionStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeThrowExceptionStatement)stmt).ToThrow, language);
            }
            else if (typeof(CodeTryCatchFinallyStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessStmt(((CodeTryCatchFinallyStatement)stmt).FinallyStatements, language);
                ProcessStmt(((CodeTryCatchFinallyStatement)stmt).TryStatements, language);
                foreach (CodeCatchClause c in ((CodeTryCatchFinallyStatement)stmt).CatchClauses)
                {
                    ProcessStmt(c.Statements, language);
                }
            }
            else if (typeof(CodeVariableDeclarationStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessExpr(((CodeVariableDeclarationStatement)stmt).InitExpression, language);
            }
            else if (typeof(CodeUsingStatement).IsAssignableFrom(stmt.GetType()))
            {
                ProcessStmt(((CodeUsingStatement)stmt).Statements, language);
                ProcessExpr(((CodeUsingStatement)stmt).UsingExpression, language);
            }

            ICustomCodeDomObject co = stmt as ICustomCodeDomObject;
            if (co != null)
                co.GenerateCode(language);
        }

        private static void ProcessExpr(CodeExpression codeExpression, LinqToCodedom.CodeDomGenerator.Language language)
        {
            if (codeExpression == null) return;

            if (typeof(CodeArgumentReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {

            }
            else if (typeof(CodeArrayCreateExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeArrayCreateExpression)codeExpression).Initializers, language);
                ProcessExpr(((CodeArrayCreateExpression)codeExpression).SizeExpression, language);
            }
            else if (typeof(CodeArrayIndexerExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeArrayIndexerExpression)codeExpression).Indices, language);
            }
            else if (typeof(CodeBaseReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {

            }
            else if (typeof(CodeBinaryOperatorExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeBinaryOperatorExpression)codeExpression).Left, language);
                ProcessExpr(((CodeBinaryOperatorExpression)codeExpression).Right, language);
            }
            else if (typeof(CodeCastExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeCastExpression)codeExpression).Expression, language);
            }
            else if (typeof(CodeDefaultValueExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodeDelegateCreateExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeDelegateCreateExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeDelegateInvokeExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeDelegateInvokeExpression)codeExpression).Parameters, language);
                ProcessExpr(((CodeDelegateInvokeExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeDirectionExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeDirectionExpression)codeExpression).Expression, language);
            }
            else if (typeof(CodeEventReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeEventReferenceExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeFieldReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeFieldReferenceExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeIndexerExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeIndexerExpression)codeExpression).Indices, language);
                ProcessExpr(((CodeIndexerExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeMethodInvokeExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeMethodInvokeExpression)codeExpression).Method, language);
                ProcessExpr(((CodeMethodInvokeExpression)codeExpression).Parameters, language);
            }
            else if (typeof(CodeMethodReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeMethodReferenceExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodeObjectCreateExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeObjectCreateExpression)codeExpression).Parameters, language);
            }
            else if (typeof(CodeParameterDeclarationExpression).IsAssignableFrom(codeExpression.GetType()))
            {

            }
            else if (typeof(CodePrimitiveExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodePropertyReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodePropertyReferenceExpression)codeExpression).TargetObject, language);
            }
            else if (typeof(CodePropertySetValueReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {

            }
            else if (typeof(CodeThisReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodeTypeOfExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodeTypeReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodeVariableReferenceExpression).IsAssignableFrom(codeExpression.GetType()))
            {
            }
            else if (typeof(CodeAssignExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeAssignExpression)codeExpression).Expression, language);
            }
            else if (typeof(CodeAsExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeAsExpression)codeExpression).Expression, language);
            }
            else if (typeof(CodeIsExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeIsExpression)codeExpression).Expression, language);
            }
            else if (typeof(CodeXorExpression).IsAssignableFrom(codeExpression.GetType()))
            {
                ProcessExpr(((CodeXorExpression)codeExpression).Left, language);
                ProcessExpr(((CodeXorExpression)codeExpression).Right, language);
            }

            ICustomCodeDomObject co = codeExpression as ICustomCodeDomObject;
            if (co != null)
                co.GenerateCode(language);
        }

        private static void ProcessExpr(IEnumerable codeExpressions, LinqToCodedom.CodeDomGenerator.Language language)
        {
            foreach (CodeExpression exp in codeExpressions)
                ProcessExpr(exp, language);
        }

        private static void ProcessStmt(IEnumerable stmts, LinqToCodedom.CodeDomGenerator.Language language)
        {
            foreach (CodeStatement stmt in stmts)
                ProcessStmt(stmt, language);
        }

        private static CodeMemberMethod ProcessMethod(CodeMemberMethod method, LinqToCodedom.CodeDomGenerator.Language language)
        {
            if (language == LinqToCodedom.CodeDomGenerator.Language.VB)
            {
                if (method.PrivateImplementationType != null)
                {
                    CodeMemberMethod newMethod = method.Clone() as CodeMemberMethod;
                    newMethod.ImplementationTypes.Add(method.PrivateImplementationType);
                    newMethod.PrivateImplementationType = null;
                    return newMethod;
                }
            }
            return method;
        }
    }
}
