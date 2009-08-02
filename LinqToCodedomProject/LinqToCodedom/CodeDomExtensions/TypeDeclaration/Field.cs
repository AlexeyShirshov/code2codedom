﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeDeclarationExtensions
    {
        #region Fields

        public static CodeTypeDeclaration AddFields(this CodeTypeDeclaration classCode, params CodeMemberField[] field)
        {
            classCode.Members_AddRange(field);

            return classCode;
        }

        public static CodeMemberField AddField(this CodeTypeDeclaration classCode,
            Type fieldType, MemberAttributes ma, string name)
        {
            var c = Define.Field(fieldType, CorrectAttributes(classCode, ma), name);
            
            classCode.Members_Add(c);

            return c;
        }

        public static CodeMemberField AddField(this CodeTypeDeclaration classCode,
            CodeTypeReference fieldType, MemberAttributes ma, string name)
        {
            var c = Define.Field(fieldType, CorrectAttributes(classCode, ma), name);

            classCode.Members_Add(c);

            return c;
        }

        public static CodeMemberField AddField(this CodeTypeDeclaration classCode,
            Type fieldType, string name)
        {
            var c = Define.Field(fieldType, CorrectAttributes(classCode, MemberAttributes.Private), name);

            classCode.Members_Add(c);

            return c;
        }

        public static CodeMemberField AddField<T>(this CodeTypeDeclaration classCode,
            Type fieldType, string name, Expression<Func<T>> exp)
        {
            var c = Define.Field(fieldType, CorrectAttributes(classCode, MemberAttributes.Private), name, exp);

            classCode.Members_Add(c);

            return c;
        }

        public static CodeMemberField AddField<T>(this CodeTypeDeclaration classCode,
            Type fieldType, MemberAttributes ma, string name, Expression<Func<T>> exp)
        {
            var c = Define.Field(fieldType, CorrectAttributes(classCode, ma), name, exp);

            classCode.Members_Add(c);

            return c;
        }

        #endregion

    }
}
