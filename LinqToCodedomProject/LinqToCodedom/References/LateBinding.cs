using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LinqToCodedom
{
    public class Base
    {
        public void Call(string name)
        {
        }

        public TReturn Call<TReturn>(string name)
        {
            return default(TReturn);
        }

        public void Property<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public T Property<T>(string name)
        {
            return default(T);
        }

        public void Call<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public TReturn Call<TReturn, TResult>(string name, Expression<Func<TResult>> paramsExp)
        {
            return default(TReturn);
        }

        public TReturn Call<TReturn, T2, TResult>(string name, Expression<Func<TReturn, TResult>> paramsExp)
        {
            return default(TReturn);
        }
    }

    public class Var : Base { }

    public class Par : Base { }

    public class This : Base { }
}
