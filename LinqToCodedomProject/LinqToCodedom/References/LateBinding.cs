using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using LinqToCodedom.CodeDomPatterns;

namespace LinqToCodedom
{
    //public class ArgsClass<T>
    //{
    //    public T Args(Expression<Action> paramsExp)
    //    {
    //        return default(T);
    //    }
    //}
    public delegate T ParamsDelegate<T>(params object[] param);
    public delegate void ParamsDelegate(params object[] param);

    public class Base
    {
        #region Properties
        
        //public void Property<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        //{
        //}

        public T Property<T>(string name)
        {
            return default(T);
        }

        #endregion

        #region Functions

        public ParamsDelegate<TReturn> Call<TReturn>(string name)
        {
            return default(ParamsDelegate<TReturn>);
        }

        //public TReturn Call<TReturn, T>(string name, Expression<Func<T>> paramsExp)
        //{
        //    return default(TReturn);
        //}

        //public TReturn Call<TReturn, T2, T>(string name, Expression<Func<T2, T>> paramsExp)
        //{
        //    return default(TReturn);
        //}

        #endregion

        //#region Functions

        //public TReturn Call<TReturn>(string name, params object[] param)
        //{
        //    return default(TReturn);
        //}

        //public TReturn Call<TReturn, T>(string name, params object[] param)
        //{
        //    return default(TReturn);
        //}

        //public TReturn Call<TReturn, T2, T>(string name, params object[] param)
        //{
        //    return default(TReturn);
        //}

        //#endregion

        #region Routines

        //public ParamsDelegate CallFunction(string name)
        //{
        //    return default(ParamsDelegate);
        //}

        public ParamsDelegate Call(string name)
        {
            return default(ParamsDelegate);
        }

        public ParamsDelegate Call()
        {
            return default(ParamsDelegate);
        }

        //public void Call(string name, params object[] param)
        //{
        //}

        //public void Call<T, TResult>(string name, params object[] param)
        //{
        //}

        #endregion

        #region Fields

        public T Field<T>(string name)
        {
            return default(T);
        }

        #endregion

        public object ArrayGet(params int[] i)
        {
            return null;
        }

        public object JaggedArrayGet(params int[] i)
        {
            return null;
        }
    }

    public class Var : Base { public Var(string name) { Name = name; } public string Name { get; protected set; } }

    //public class Par : Base { }

    public class This : Base 
    {
        public ParamsDelegate Raise(string name)
        {
            return default(ParamsDelegate);
        }
    }

    public interface IDynType
    {
        string SetType(string type);
        string SetType(Type type);
        string SetType(System.CodeDom.CodeTypeReference type);

        //OperatorType SetOperatorType(string type);
        //OperatorType SetOperatorType(Type type);
        //OperatorType SetOperatorType(System.CodeDom.CodeTypeReference type);
    }

    public interface IRefParam { }
    public interface IOutParam { }

    public struct DynType : IDynType
    {
        public string SetType(string type) { return string.Empty; }
        public string SetType(Type type) { return string.Empty; }
        public string SetType(System.CodeDom.CodeTypeReference type) { return string.Empty; }

        //public OperatorType SetOperatorType(string type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(Type type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(System.CodeDom.CodeTypeReference type) { return OperatorType.Explicit; }
    }

    public struct DynTypeRef : IDynType, IRefParam
    {
        public string SetType(string type) { return string.Empty; }
        public string SetType(Type type) { return string.Empty; }
        public string SetType(System.CodeDom.CodeTypeReference type) { return string.Empty; }

        //public OperatorType SetOperatorType(string type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(Type type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(System.CodeDom.CodeTypeReference type) { return OperatorType.Explicit; }
    }

    public struct DynTypeOut : IDynType, IOutParam
    {
        public string SetType(string type) { return string.Empty; }
        public string SetType(Type type) { return string.Empty; }
        public string SetType(System.CodeDom.CodeTypeReference type) { return string.Empty; }

        //public OperatorType SetOperatorType(string type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(Type type) { return OperatorType.Explicit; }
        //public OperatorType SetOperatorType(System.CodeDom.CodeTypeReference type) { return OperatorType.Explicit; }
    }

    public class RefParam<T> : IRefParam { };
    public class OutParam<T> : IOutParam { };
}
