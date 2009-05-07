using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;
using System;
using System.Linq;

namespace LinqToCodedom.Extensions
{
    public static partial class CodeTypeDeclarationExtensions
    {
        internal static CodeTypeDeclaration Members_Add(this CodeTypeDeclaration classCode, CodeTypeMember member)
        {
            //if (typeof(CodeMemberProperty).IsAssignableFrom(member.GetType()))
            //{
            //    if (classCode.IsInterface)
            //    {
            //        CodeMemberProperty prop = member as CodeMemberProperty;
            //        prop.HasGet 
            //    }
            //}

            member.UserData["parent"] = classCode;

            classCode.Members.Add(member);

            return classCode;
        }

        internal static CodeTypeDeclaration Members_AddRange(this CodeTypeDeclaration classCode, CodeTypeMember[] members)
        {
            foreach (CodeTypeMember member in members)
            {
                classCode.Members_Add(member);
            }

            return classCode;
        }

        internal static CodeTypeDeclaration GetDeclaration(this CodeTypeDeclaration member)
        {
            return member;
        }

        public static CodeNamespace GetNamespace(this CodeTypeDeclaration type)
        {
            return type.UserData["ns"] as CodeNamespace;
        }

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration classCode, CodeTypeDeclaration codeType)
        {
            classCode.Members_Add(codeType);

            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, string type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, Type type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Inherits(this CodeTypeDeclaration classCode, CodeTypeReference type)
        {
            classCode.BaseTypes.Add(type);
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params string[] type)
        {
            classCode.BaseTypes.AddRange(type.Select((t)=>new CodeTypeReference(t)).ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params Type[] type)
        {
            classCode.BaseTypes.AddRange(type.Select((t) => new CodeTypeReference(t)).ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params CodeTypeReference[] type)
        {
            classCode.BaseTypes.AddRange(type);
            return classCode;
        }

        private static MemberAttributes CorrectAttributes(CodeTypeDeclaration classDecl, MemberAttributes ma)
        {
            if (classDecl.IsInterface/* || (classDecl.Attributes & MemberAttributes.Abstract) == MemberAttributes.Abstract*/)
            {
                ma |= MemberAttributes.Abstract;
                ma &= ~MemberAttributes.Public;
            }
            else if (classDecl.IsStruct)
            {
                ma |= MemberAttributes.Final;
            }
            return ma;
        }

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

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration @class, string className)
        {
            var c = Define.Class(className);

            @class.GetNamespace().Types.Add(c);

            return c;
        }

        #endregion

    }
}
