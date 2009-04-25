using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LinqToCodedom
{
    public class ArgsClass<T>
    {
        public T Args(Expression<Action> paramsExp)
        {
            return default(T);
        }
    }

    public class Base
    {
        public void Call(string name)
        {
        }

        public void Property<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public T Property<T>(string name)
        {
            return default(T);
        }

        public void Call(string name, Expression<Action> paramsExp)
        {
        }

        public void Call<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public ArgsClass<TReturn> Call<TReturn>(string name)
        {
            return default(ArgsClass<TReturn>);
        }

        public TReturn Call<TReturn, T>(string name, Expression<Func<T>> paramsExp)
        {
            return default(TReturn);
        }

        public TReturn Call<TReturn, T2, T>(string name, Expression<Func<T2, T>> paramsExp)
        {
            return default(TReturn);
        }
    }

    public class Var : Base { }

    public class Par : Base { }

    public class This : Base { }
}
