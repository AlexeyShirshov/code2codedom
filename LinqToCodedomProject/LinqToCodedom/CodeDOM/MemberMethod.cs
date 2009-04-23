using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom
{
    public static class CodeMemberMethodExtensions
    {
        public static CodeMemberMethod Implements(this CodeMemberMethod method, Type t)
        {
            method.ImplementationTypes.Add(t);
            return method;
        }
    }
}
