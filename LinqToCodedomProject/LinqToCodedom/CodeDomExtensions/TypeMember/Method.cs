using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeMemberExtensions
    {
        public static CodeTypeDeclaration AddMethod(this CodeTypeMember member, CodeMemberMethod methodBody)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(methodBody);
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeMember member,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod<T, T2>(this CodeTypeMember member,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, T2, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeMember member,
            string returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod(this CodeTypeMember member,
            string returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod(this CodeTypeMember member,
            Type returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeMember member,
            MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod<T, T2>(this CodeTypeMember member,
            MemberAttributes ma,
            Expression<Func<T, T2, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(ma, paramsAndName, statements);
        }

        public static CodeMemberMethod AddMethod(this CodeTypeMember member,
            MemberAttributes ma, Expression<Func<string>> paramsAndName,
            params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(ma, paramsAndName, statements);
        }

    }
}
