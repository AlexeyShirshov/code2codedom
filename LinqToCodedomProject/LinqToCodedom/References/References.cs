using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToCodedom
{
    public class VarRef<T>
    {
        public T v;

        public static implicit operator T(VarRef<T> d)
        {
            return default(T);
        }
    }

    public class ParamRef<T>
    {
        public T v;

        public static implicit operator T(ParamRef<T> d)
        {
            return default(T);
        }
    }

    public class Mem<T>
    {
        public T v;
    }
}
