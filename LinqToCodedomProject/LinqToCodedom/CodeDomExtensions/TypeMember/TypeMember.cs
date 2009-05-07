using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeMemberExtensions
    {
        #region CodeTypeMember

        internal static CodeTypeDeclaration GetDeclaration(this CodeTypeMember member)
        {
            if (member is CodeTypeDeclaration)
                return member as CodeTypeDeclaration;
            else
                return member.UserData["parent"] as CodeTypeDeclaration;
        }

        public static CodeTypeMember Comment(this CodeTypeMember member, params CodeCommentStatement[] comments)
        {
            member.Comments.AddRange(comments);
            return member;
        }

        public static CodeTypeMember Comment(this CodeTypeMember member, params string[] comments)
        {
            member.Comments.AddRange(comments.Select((c) => new CodeCommentStatement(c)).ToArray());
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
            member.CustomAttributes.AddRange(attributes.Select((a) => new CodeAttributeDeclaration(new CodeTypeReference(a))).ToArray());
            return member;
        }

        public static CodeTypeMember AddAttribute(this CodeTypeMember member, params string[] attributes)
        {
            member.CustomAttributes.AddRange(attributes.Select((a) => new CodeAttributeDeclaration(a)).ToArray());
            return member;
        }

        public static CodeTypeDeclaration AddClass(this CodeTypeMember member, string className)
        {
            var c = Define.Class(className);

            member.GetDeclaration().GetNamespace().Types.Add(c);

            return c;
        }

        #endregion

    }
}
