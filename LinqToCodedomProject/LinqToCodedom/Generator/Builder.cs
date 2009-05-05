using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToCodedom.Visitors;
using System.CodeDom;
using System.Linq.Expressions;
using System.ComponentModel;

namespace LinqToCodedom.Generator
{
    public static partial class Builder
    {
        public class NilClass { }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class CodeNilExpression : CodeExpression { };
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class CodeThisExpression : CodeExpression { };
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class CodeBaseExpression : CodeExpression { };
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class CodeVarExpression : CodeVariableReferenceExpression
        {
            public CodeVarExpression(string name) : base(name) { }
        };
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class CodeArgsInvoke : CodeMethodInvokeExpression
        {
            public CodeArgsInvoke(CodeExpression target, string methodName)
                : base(target, methodName)
            {
            }
        };

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

        public static Var VarRef(string name)
        {
            return null;
        }

        //public static T ParamRef<T>(string name)
        //{
        //    return default(T);
        //}

        internal static T GetMethodName<T>(LambdaExpression exp, CodeParameterDeclarationExpressionCollection pars)
        {
            foreach (var p in exp.Parameters)
            {
                var par = new CodeParameterDeclarationExpression(p.Type, p.Name);
                pars.Add(par);
            }
            return Eval<T>(exp.Body);
        }

        public static CodeStatement[] CombineStmts(params CodeStatement[] stmts)
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

        public static object Eval(Expression exp)
        {
            return Eval<object>(exp);
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

        #region Properties

        //public void Property<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        //{
        //}

        public static T Property<T>(object target, string name)
        {
            return default(T);
        }

        #endregion

        #region Functions

        public static ParamsDelegate<TReturn> Call<TReturn>(object target, string name)
        {
            return default(ParamsDelegate<TReturn>);
        }

        //public static TReturn Invoke<TReturn, T>(object target, string name, params object[] param)
        //{
        //    return default(TReturn);
        //}

        //public static TReturn Invoke<TReturn, T2, T>(object target, string name, params object[] param)
        //{
        //    return default(TReturn);
        //}

        #endregion

        #region Routines

        public static ParamsDelegate Call(object target, string name)
        {
            return default(ParamsDelegate);
        }

        //public static void Call(object target, string name, params object[] param)
        //{
        //}

        //public static void Call<T, TResult>(object target, string name, params object[] param)
        //{
        //}

        //public static void Call<TResult>(object target, string name, params object[] param)
        //{
        //}

        #endregion

        #region Fields

        public static T Field<T>(string name)
        {
            return default(T);
        }

        #endregion

        #region New
        public static CodeExpression @new(string type)
        {
            throw new NotSupportedException();
        }

        public static CodeExpression @new(string type, params object[] param)
        {
            throw new NotSupportedException();
        }

        public static CodeExpression @new(Type type)
        {
            throw new NotSupportedException();
        }

        public static CodeExpression @new(Type type, params object[] param)
        {
            throw new NotSupportedException();
        }

        public static CodeExpression @new(CodeTypeReference type)
        {
            throw new NotSupportedException();
        }

        public static CodeExpression @new(CodeTypeReference type, params object[] param)
        {
            throw new NotSupportedException();
        }

        #endregion

        public static CodeTypeReference TypeRef(string type, params string[] types)
        {
            return new CodeTypeReference(type,
                types.Select((t) => new CodeTypeReference(t)).ToArray());
        }

        public static CodeTypeReference generic(Type type, params Type[] types)
        {
            var d = new CodeTypeReference(type);
            d.TypeArguments.AddRange(types.Select((t) => new CodeTypeReference(t)).ToArray());
            return d;
        }

        public static CodeExpression[] ToArray(this CodeExpressionCollection col)
        {
            CodeExpression[] param = new CodeExpression[col.Count];
            col.CopyTo(param, 0);
            return param;
        }

        //public static System.Collections.ObjectModel.ReadOnlyCollection<Expression> ToReadOnlyList(this IEnumerable<Expression> col)
        //{
        //    return new System.Collections.ObjectModel.ReadOnlyCollection<Expression>(col.ToList());
        //}
    }
}
