using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Generator
{
    public class LambdaParam
    {
        public string Name { get; set; }
        public CodeTypeReference Type { get; set; }

        public LambdaParam(string name)
        {
            Name = name;
        }

        public LambdaParam(CodeTypeReference type, string name)
        {
            Name = name;
            Type = type;
        }

        public LambdaParam(string type, string name)
            : this(new CodeTypeReference(type), name)
        {
        }

        public LambdaParam(Type type, string name)
            : this(new CodeTypeReference(type), name)
        {
        }
    }
    //public class LambdaParams
    //{
        

    //    //private readonly List<Params> _params = new List<Params>();

    //    public LambdaParams Param(string name)
    //    {
    //        //_params.Add(new Params(name));
    //        return this;
    //    }

    //    public LambdaParams Param(CodeTypeReference type, string name)
    //    {
    //        //_params.Add(new Params(name, type));
    //        return this;
    //    }

    //    public T Returns<T>()
    //    {
    //        return default(T);
    //    }

    //    //public void Returns()
    //    //{
    //    //}
    //}

    public static partial class CodeDom
    {

        public static Expression<Func<T>> Lambda<T>(Expression<Func<T>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Func<T, T2>> Lambda<T, T2>(Expression<Func<T, T2>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Func<T, T2, T3>> Lambda<T, T2, T3>(Expression<Func<T, T2, T3>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Func<T, T2, T3, T4>> Lambda<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Action> Lambda(Expression<Action> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Action<T>> Lambda<T>(Expression<Action<T>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Action<T, T2>> Lambda<T, T2>(Expression<Action<T, T2>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Action<T, T2, T3>> Lambda<T, T2, T3>(Expression<Action<T, T2, T3>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }

        public static Expression<Action<T, T2, T3, T4>> Lambda<T, T2, T3, T4>(Expression<Action<T, T2, T3, T4>> exp, params LambdaParam[] lambdaParams)
        {
            return exp;
        }
    }
}
