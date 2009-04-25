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
            get { return null; }
        }

        public static This @this
        {
            get { return default(This); }
        }

        public static This @base
        {
            get { return default(This); }
        }

        public static void Seq(params object[] args)
        {
        }

        public static T NamedSeq<T>(T anonymousTypeDeclaration)
        {
            return default(T);
        }

        public static T VarRef<T>(string name)
        {
            return default(T);
        }

        public static T ParamRef<T>(string name)
        {
            return default(T);
        }

        private static T GetMethodName<T>(LambdaExpression exp, CodeParameterDeclarationExpressionCollection pars)
        {
            foreach (var p in exp.Parameters)
            {
                var par = new CodeParameterDeclarationExpression(p.Type, p.Name);
                pars.Add(par);
            }
            return Eval<T>(exp.Body);
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

        #region Default

        public static CodeDefaultValueExpression @default(CodeTypeReference type)
        {
            return new CodeDefaultValueExpression(type);
        }

        public static CodeDefaultValueExpression @default(string type)
        {
            return new CodeDefaultValueExpression(new CodeTypeReference(type));
        }

        public static CodeDefaultValueExpression @default(Type type)
        {
            return new CodeDefaultValueExpression(new CodeTypeReference(type));
        }

        #endregion

        #region Method
        public static CodeMemberMethod Method<T>(Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            CodeMemberMethod method = new CodeMemberMethod()
            {
                ReturnType = returnType == null ? null : new CodeTypeReference(returnType),
                Attributes = ma
            };

            method.Name = GetMethodName<string>(paramsAndName, method.Parameters);
            method.Statements.AddRange(statements);

            return method;
        }

        #endregion

        #region Properties

        public static CodeMemberProperty GetProperty(Type propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
            };

            c.GetStatements.AddRange(statements);

            return c;
        }

        public static CodeMemberProperty Property(Type propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
            };

            c.GetStatements.AddRange(getStatements);
            c.SetStatements.AddRange(setStatements);

            return c;
        }

        #endregion

        #region Fields
        public static CodeMemberField Field(Type fieldType, MemberAttributes ma, string name,
            CodeExpression initExpression)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
                InitExpression = initExpression,
            };

            return c;
        }
        #endregion

        #region Event
        public static CodeMemberEvent Event(Type delegateType, MemberAttributes ma, string name)
        {
            var c = new CodeMemberEvent()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(delegateType),
            };

            return c;
        }
        #endregion

        #region Delegate
        public static CodeTypeDelegate Delegate<T>(Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName)
        {
            var c = new CodeTypeDelegate()
            {
                Attributes = ma,
                ReturnType = new CodeTypeReference(returnType),
            };

            c.Name = GetMethodName<string>(paramsAndName, c.Parameters);

            return c;
        }
        #endregion

        #region Ctor
        public static CodeConstructor Ctor<T>(Expression<Func<T, MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            var c = new CodeConstructor();

            c.Attributes = GetMethodName<MemberAttributes>(paramsAndAccessLevel, c.Parameters);
            c.Statements.AddRange(statements);

            return c;
        }
        #endregion

        #region Attribute

        public static CodeAttributeDeclaration Attribute(CodeTypeReference type)
        {
            return new CodeAttributeDeclaration(type);
        }

        public static CodeAttributeDeclaration Attribute(string type)
        {
            return new CodeAttributeDeclaration(type);
        }

        public static CodeAttributeDeclaration Attribute<T>(CodeTypeReference type,
            Expression<Func<T>> anonymType)
        {
            var c = new CodeAttributeDeclaration(type);
            InitAttributeArgs(anonymType, c);
            return c;

        }

        public static CodeAttributeDeclaration Attribute(CodeTypeReference type,
            params object[] args)
        {
            var c = new CodeAttributeDeclaration(type);
            c.Arguments.AddRange(args.Select((a) => new CodeAttributeArgument(new CodePrimitiveExpression(a))).ToArray());
            return c;

        }

        public static CodeAttributeDeclaration Attribute<T>(string type,
            Expression<Func<T>> anonymType)
        {
            var c = new CodeAttributeDeclaration(type);

            InitAttributeArgs(anonymType, c);

            return c;

        }

        private static void InitAttributeArgs(Expression anonymType, CodeAttributeDeclaration c)
        {
            object o = Eval<object>(anonymType);

            foreach (System.Reflection.PropertyInfo pi in
                o.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                c.Arguments.Add(new CodeAttributeArgument(pi.Name,
                    new CodePrimitiveExpression(pi.GetValue(o, null))
                ));
            }
        }

        public static CodeAttributeDeclaration Attribute(string type,
            params object[] args)
        {
            var c = new CodeAttributeDeclaration(type);
            c.Arguments.AddRange(args.Select((a) => new CodeAttributeArgument(new CodePrimitiveExpression(a))).ToArray());
            return c;

        }

        #endregion
    }
}
