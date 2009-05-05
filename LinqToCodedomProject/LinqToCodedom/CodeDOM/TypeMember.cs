using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;

namespace LinqToCodedom
{
    public static class CodeTypeMemberExtensions
    {
        #region Methods

        public static CodeTypeMember AddMethod(this CodeTypeMember member, CodeMemberMethod methodBody)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(methodBody);
        }

        public static CodeTypeMember AddMethod<T>(this CodeTypeMember member,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeTypeMember AddMethod<T>(this CodeTypeMember member,
            string returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeTypeMember AddMethod(this CodeTypeMember member,
            string returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeTypeMember AddMethod(this CodeTypeMember member,
            Type returnType, MemberAttributes ma,
            Expression<Func<string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(returnType, ma, paramsAndName, statements);
        }

        public static CodeTypeMember AddMethod<T>(this CodeTypeMember member,
            MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(ma, paramsAndName, statements);
        }

        public static CodeTypeMember AddMethod(this CodeTypeMember member,
            MemberAttributes ma, Expression<Func<string>> paramsAndName,
            params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddMethod(ma, paramsAndName, statements);
        }

        #endregion

        #region Properties

        public static CodeTypeMember AddProperty(this CodeTypeMember member, CodeMemberProperty property)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(property);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member, 
            string propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name, fieldName);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member,
            Type propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name, fieldName);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member,
            string propertyType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member,
            Type propertyType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member, 
            Type propertyType, MemberAttributes ma, string name,
                CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name, getStatements, setStatements);
        }

        public static CodeTypeMember AddProperty(this CodeTypeMember member,
            string propertyType, MemberAttributes ma, string name,
                CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name, getStatements, setStatements);
        }

        public static CodeTypeMember AddGetProperty(this CodeTypeMember member,
            Type propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddGetProperty(propertyType, ma, name, statements);
        }

        public static CodeTypeMember AddGetProperty(this CodeTypeMember member,
           Type propertyType, MemberAttributes ma, string name, string fieldName)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddGetProperty(propertyType, ma, name, fieldName);
        }

        public static CodeTypeMember AddGetProperty(this CodeTypeMember member,
            string propertyType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddGetProperty(propertyType, ma, name);
        }
        
        public static CodeTypeMember Property(this CodeTypeMember member,
            Type propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddProperty(propertyType, ma, name, getStatements, setStatements);
        }

        #endregion

        #region Fields

        public static CodeTypeMember AddFields(this CodeTypeMember member, params CodeMemberField[] field)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddFields(field);
        }

        public static CodeTypeMember AddField(this CodeTypeMember member,
            Type fieldType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddField(fieldType, ma, name);
        }

        #endregion

        #region Ctor

        public static CodeTypeMember AddCtor(this CodeTypeMember member, CodeConstructor ctor)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddCtor(ctor);
        }

        public static CodeTypeMember AddCtor<T>(this CodeTypeMember member,
            Expression<Func<T, MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddCtor(paramsAndAccessLevel, statements);
        }

        #endregion

        #region Event

        public static CodeTypeDeclaration AddEvents(this CodeTypeMember member, params CodeMemberEvent[] @event)
        {
            var classCode = member.GetDeclaration();

            classCode.AddEvents(@event);

            return classCode;
        }

        public static CodeMemberEvent AddEvent(this CodeTypeMember member,
            string delegateType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddEvent(delegateType, ma, name);
        }

        public static CodeMemberEvent AddEvent(this CodeTypeMember member,
            Type delegateType, MemberAttributes ma, string name)
        {
            var classCode = member.GetDeclaration();

            return classCode.AddEvent(delegateType, ma, name);
        }

        #endregion
    }
}
