using System;
using System.CodeDom;

namespace LinqToCodedom.Extensions
{
    public static class CodeMemberPropertyExtensions
    {
        #region Property

        public static CodeMemberProperty Implements(this CodeMemberProperty property, CodeTypeReference t)
        {
            property.ImplementationTypes.Add(t);
            return property;
        }

        public static CodeMemberProperty Implements(this CodeMemberProperty property, Type t)
        {
            property.ImplementationTypes.Add(t);
            return property;
        }

        public static CodeMemberProperty Implements(this CodeMemberProperty property, string t)
        {
            property.ImplementationTypes.Add(t);
            return property;
        }

        #endregion
    }
}
