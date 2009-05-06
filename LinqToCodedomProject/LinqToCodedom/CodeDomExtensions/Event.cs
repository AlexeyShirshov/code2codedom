using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.Extensions
{
    public static class CodeMemberEventExtensions
    {
        public static CodeMemberEvent Implements(this CodeMemberEvent @event, Type t)
        {
            @event.ImplementationTypes.Add(t);
            return @event;
        }

        public static CodeMemberEvent Implements(this CodeMemberEvent @event, string t)
        {
            @event.ImplementationTypes.Add(t);
            return @event;
        }
    }
}
