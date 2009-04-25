using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;
using System;

namespace LinqToCodedom
{
    public static class CodeTypeDeclarationExtensions
    {
        #region Methods
        public static CodeTypeDeclaration AddMethod(this CodeTypeDeclaration classCode, CodeMemberMethod methodBody)
        {
            classCode.Members.Add(methodBody);

            return classCode;
        }

        public static CodeTypeDeclaration AddMethod<T>(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            classCode.Members.Add(Builder.Method(returnType, ma, paramsAndName, statements));

            return classCode;
        }

        #endregion

        #region Properties
        public static CodeTypeDeclaration AddProperty(this CodeTypeDeclaration classCode, CodeMemberProperty property)
        {
            classCode.Members.Add(property);

            return classCode;
        }

        public static CodeTypeDeclaration GetProperty(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            classCode.Members.Add(Builder.GetProperty(propertyType, ma, name, statements));

            return classCode;
        }

        public static CodeTypeDeclaration Property(this CodeTypeDeclaration classCode,
            Type propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            classCode.Members.Add(Builder.Property(propertyType, ma, name, getStatements, setStatements));

            return classCode;
        }

        #endregion

        #region Fields

        public static CodeTypeDeclaration AddField(this CodeTypeDeclaration classCode, CodeMemberField field)
        {
            classCode.Members.Add(field);

            return classCode;
        }

        #endregion

        #region Event
         
        public static CodeTypeDeclaration AddField(this CodeTypeDeclaration classCode, CodeMemberEvent @event)
        {
            classCode.Members.Add(@event);

            return classCode;
        }

        #endregion

        #region Delegate

        public static CodeTypeDeclaration AddDelegate(this CodeTypeDeclaration classCode, CodeTypeDelegate @delegate)
        {
            classCode.Members.Add(@delegate);

            return classCode;
        }

        public static CodeTypeDeclaration AddDelegate<T>(this CodeTypeDeclaration classCode,
            Type returnType, MemberAttributes ma, Expression<Func<T, string>> paramsAndName)
        {
            classCode.Members.Add(Builder.Delegate(returnType, ma, paramsAndName));

            return classCode;
        }

        #endregion

        #region Ctor

        public static CodeTypeDeclaration AddCtor(this CodeTypeDeclaration @class, CodeConstructor ctor)
        {
            @class.Members.Add(ctor);

            return @class;
        }

        public static CodeTypeDeclaration AddCtor<T>(this CodeTypeDeclaration @class,
            Expression<Func<T, MemberAttributes>> paramsAndAccessLevel,
            params CodeStatement[] statements)
        {
            @class.Members.Add(Builder.Ctor(paramsAndAccessLevel, statements));

            return @class;
        }

        #endregion

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration classCode, CodeTypeDeclaration codeType)
        {
            classCode.Members.Add(codeType);

            return classCode;
        }
    }
}
