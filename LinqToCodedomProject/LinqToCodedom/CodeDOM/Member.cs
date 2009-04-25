using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom
{
    public static class CodeMemberMethodExtensions
    {
        #region Method
        public static CodeMemberMethod Implements(this CodeMemberMethod method, Type t)
        {
            method.ImplementationTypes.Add(t);
            return method;
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod method, string paramName)
        {
            method.TypeParameters.Add(paramName);
            return method;
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod method, string paramName, 
            params string[] constraints)
        {
            var p = new CodeTypeParameter(paramName);
            p.Constraints.AddRange(constraints.Select((t) => new CodeTypeReference(t)).ToArray());
            method.TypeParameters.Add(p);
            return method;
        }

        public static CodeMemberMethod Generic(this CodeMemberMethod method, string paramName,
            params Type[] constraints)
        {
            var p = new CodeTypeParameter(paramName);
            p.Constraints.AddRange(constraints.Select((t) => new CodeTypeReference(t)).ToArray());
            method.TypeParameters.Add(p);
            return method;
        }
        #endregion

        #region Class
        public static CodeTypeDeclaration Generic(this CodeTypeDeclaration @class, string paramName)
        {
            @class.TypeParameters.Add(paramName);
            return @class;
        }

        public static CodeTypeDeclaration Generic(this CodeTypeDeclaration @class, string paramName,
            params string[] constraints)
        {
            var p = new CodeTypeParameter(paramName);
            p.Constraints.AddRange(constraints.Select((t) => new CodeTypeReference(t)).ToArray());
            @class.TypeParameters.Add(p);
            return @class;
        }

        public static CodeTypeDeclaration Generic(this CodeTypeDeclaration @class, string paramName,
            params Type[] constraints)
        {
            var p = new CodeTypeParameter(paramName);
            p.Constraints.AddRange(constraints.Select((t) => new CodeTypeReference(t)).ToArray());
            @class.TypeParameters.Add(p);
            return @class;
        }
        #endregion

        public static CodeMemberProperty Implements(this CodeMemberProperty property, Type t)
        {
            property.ImplementationTypes.Add(t);
            return property;
        }

        public static CodeTypeMember Comment(this CodeTypeMember member, params CodeCommentStatement[] comments)
        {
            member.Comments.AddRange(comments);
            return member;
        }

        public static CodeTypeMember Comment(this CodeTypeMember member, params string[] comments)
        {
            member.Comments.AddRange(comments.Select((c)=>new CodeCommentStatement(c)).ToArray());
            return member;
        }

        public static CodeTypeMember AddAttribute(this CodeTypeMember member, params CodeAttributeDeclaration[] attributes)
        {
            member.CustomAttributes.AddRange(attributes);
            return member;
        }

        public static CodeTypeMember AddAttribute(this CodeTypeMember member, params Type[] attributes)
        {
            member.CustomAttributes.AddRange(attributes.Select((a)=>new CodeAttributeDeclaration(new CodeTypeReference(a))).ToArray());
            return member;
        }

        public static CodeTypeMember AddAttribute(this CodeTypeMember member, params string[] attributes)
        {
            member.CustomAttributes.AddRange(attributes.Select((a) => new CodeAttributeDeclaration(a)).ToArray());
            return member;
        }
    }
}
