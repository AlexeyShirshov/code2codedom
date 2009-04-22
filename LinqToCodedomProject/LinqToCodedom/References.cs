using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToCodedom
{
    public class Var<T>
    {
        public T v;

        public static implicit operator T(Var<T> d)
        {
            return default(T);
        }
    }

    public class Par<T>
    {
        public T v;

        public static implicit operator T(Par<T> d)
        {
            return default(T);
        }
    }

    public class Mem<T>
    {
        public T v;
    }
}
