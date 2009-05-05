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
        //public static CodeMemberMethod Clone(this CodeMemberMethod method)
        //{
        //    var c = new CodeMemberMethod()
        //    {
        //        Name = method.Name,
        //        Attributes = method.Attributes,
        //        ReturnType = method.ReturnType,
        //        PrivateImplementationType = method.PrivateImplementationType,
        //        CustomAttributes = method.CustomAttributes,
        //    };
        //    c.Parameters.AddRange(method.Parameters);
        //    c.ImplementationTypes.AddRange(method.ImplementationTypes);
        //    c.Comments.AddRange(method.Comments);
        //    c.ReturnTypeCustomAttributes.AddRange(method.ReturnTypeCustomAttributes);
        //    c.Statements.AddRange(method.Statements);
        //    c.TypeParameters.AddRange(method.TypeParameters);
            
        //    return c;
        //}

        public static CodeMemberMethod Implements(this CodeMemberMethod method, Type t)
        {
            if ((method.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
                method.PrivateImplementationType = new CodeTypeReference(t);
            else
                method.ImplementationTypes.Add(t);

            return method;
        }

        public static CodeMemberMethod Implements(this CodeMemberMethod method, CodeTypeReference t)
        {
            if ((method.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
                method.PrivateImplementationType = t;
            else
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

        #region Property
        
        public static CodeMemberProperty Implements(this CodeMemberProperty property, Type t)
        {
            property.ImplementationTypes.Add(t);
            return property;
        }

        #endregion

        internal static CodeTypeDeclaration GetDeclaration(this CodeTypeMember member)
        {
            if (member is CodeTypeDeclaration)
                return member as CodeTypeDeclaration;
            else
                return member.UserData["parent"] as CodeTypeDeclaration;
        }

        internal static CodeTypeDeclaration GetDeclaration(this CodeTypeDeclaration member)
        {
            return member;
        }

        #region CodeTypeMember

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

        public static CodeTypeMember Document(this CodeTypeMember member, params string[] comments)
        {
            member.Comments.AddRange(comments.Select((c) => new CodeCommentStatement(c, true)).ToArray());
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

        #endregion

    }
}
