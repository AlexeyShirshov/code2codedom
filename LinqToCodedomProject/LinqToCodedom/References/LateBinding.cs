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

        public T Call<T>(string name)
        {
            return default(T);
        }

        public void Property(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public T Property<T>(string name)
        {
            return default(T);
        }

        public void Call<T, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
        }

        public T Call<T, T2, TResult>(string name, Expression<Func<T, TResult>> paramsExp)
        {
            return default(T);
        }
    }

    public class Var : Base { }

    public class Par : Base { }
}
