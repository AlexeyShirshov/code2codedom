using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.Generator
{
    public static partial class Define
    {
        #region Event

        public static CodeMemberEvent Event(Type delegateType, MemberAttributes ma, string name)
        {
            var c = new CodeMemberEvent()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(delegateType),
            };

            return c;
        }

        public static CodeMemberEvent Event(string delegateType, MemberAttributes ma, string name)
        {
            var c = new CodeMemberEvent()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(delegateType),
            };

            return c;
        }

        #endregion
    }
}
