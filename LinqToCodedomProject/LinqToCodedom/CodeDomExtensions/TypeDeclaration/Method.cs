using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeDeclarationExtensions
    {
        #region Methods

        public static CodeTypeDeclaration AddMethod(this CodeTypeDeclaration classCode, CodeMemberMethod methodBody)
        {
            classCode.Members_Add(methodBody);

            return classCode;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            string returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            string returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            CodeTypeReference returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(returnType, CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod<T>(this CodeTypeDeclaration classCode,
            MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod<T, T2>(this CodeTypeDeclaration classCode,
            MemberAttributes ma,
            Expression<Func<T, T2, string>> paramsAndName, params CodeStatement[] statements)
        {
            var meth = Define.Method(CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration classCode,
            MemberAttributes ma, Expression<Func<string>> paramsAndName,
            params CodeStatement[] statements)
        {
            var meth = Define.Method(CorrectAttributes(classCode, ma), paramsAndName, statements);

            classCode.Members_Add(meth);

            return meth;
        }

        #endregion

    }
}
