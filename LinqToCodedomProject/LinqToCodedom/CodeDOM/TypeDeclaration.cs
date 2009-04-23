using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;
using System;

namespace LinqToCodedom
{
    public static class CodeTypeDeclarationExtensions
    {
        public static CodeTypeDeclaration AddMethod(this CodeTypeDeclaration classCode, CodeMemberMethod methodBody)
        {
            classCode.Members.Add(methodBody);

            return classCode;
        }

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration classCode, CodeTypeDeclaration codeType)
        {
            classCode.Members.Add(codeType);

            return classCode;
        }

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

        public static CodeTypeDeclaration AddMethod<T>(this CodeTypeDeclaration classCode, 
            Type returnType, MemberAttributes ma,
            Expression<Func<T, string>> paramsAndName, params CodeStatement[] statements)
        {
            classCode.Members.Add(Builder.Method(returnType, ma, paramsAndName, statements));

            return classCode;
        }
    }
}
