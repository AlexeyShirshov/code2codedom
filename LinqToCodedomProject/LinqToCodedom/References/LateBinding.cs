using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

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

        public ParamsDelegate Call(string name)
        {
            return default(ParamsDelegate);
        }

        //public void Call(string name)
        //{
        //}

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
    }

    public class Var : Base { }

    public class Par : Base { }

    public class This : Base { }
}
