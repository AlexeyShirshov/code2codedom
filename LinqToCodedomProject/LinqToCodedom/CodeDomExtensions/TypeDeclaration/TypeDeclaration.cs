﻿using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Generator;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
            var s = type.Select((t) => new CodeTypeReference(t));
            foreach (var item in s)
            {
                item.UserData["linq2codedom:interface"] = true;
            }
            classCode.BaseTypes.AddRange(s.ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params Type[] type)
        {
            var s = type.Select((t) => new CodeTypeReference(t));
            foreach (var item in s)
            {
                item.UserData["linq2codedom:interface"] = true;
            }
            classCode.BaseTypes.AddRange(s.ToArray());
            return classCode;
        }

        public static CodeTypeDeclaration Implements(this CodeTypeDeclaration classCode, params CodeTypeReference[] type)
        {
            foreach (var item in type)
            {
                item.UserData["linq2codedom:interface"] = true;
            }
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

        #region Enum

        public static CodeTypeDeclaration AddEnum(this CodeTypeDeclaration @class, CodeTypeDeclaration enumType)
        {
            @class.Members_Add(enumType);

            return @class;
        }

        public static CodeTypeDeclaration AddEnum(this CodeTypeDeclaration @class, string enumName)
        {
            var e = Define.Enum(enumName);

            @class.AddEnum(e);

            return e;
        }

        #endregion

        public static CodeTypeDeclaration AddMember(this CodeTypeDeclaration @class, CodeTypeMember member)
        {
            @class.Members_Add(member);

            return @class;
        }

        public static bool IsEquals(this CodeTypeReference typeRef, CodeTypeReference type)
        {
            if (typeRef == null)
            {
                if (type == null)
                    return true;
            }
            else if (type != null)
            {
                if (typeRef.BaseType == type.BaseType)
                {
                    if ((typeRef.TypeArguments == null || typeRef.TypeArguments.Count == 0) &&
                        (type.TypeArguments == null || type.TypeArguments.Count == 0))
                    {
                        return true;
                    }
                    else if (typeRef.TypeArguments != null && type.TypeArguments != null)
                    {
                        return typeRef.TypeArguments.Cast<CodeTypeReference>().All(outer => type.TypeArguments.Cast<CodeTypeReference>().Any(inner => inner.IsEquals(outer)));
                    }
                }
            }
            return false;
        }
    }

    public class CodeTypeReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<CodeTypeReference>
    {

        bool IEqualityComparer.Equals(object x, object y)
        {
            return Equals(x as CodeTypeReference, y as CodeTypeReference);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode(obj as CodeTypeReference);
        }

        public bool Equals(CodeTypeReference x, CodeTypeReference y)
        {
            return x.IsEquals(y);
        }

        public int GetHashCode(CodeTypeReference typeRef)
        {
            if (typeRef != null && !string.IsNullOrEmpty(typeRef.BaseType))
            {
                var r = typeRef.BaseType.GetHashCode();

                if (typeRef.TypeArguments != null)
                    foreach (CodeTypeReference t in typeRef.TypeArguments)
                    {
                        r |= new CodeTypeReferenceEqualityComparer().GetHashCode(t);
                    }

                return r;
            }

            return 0;

        }
    }
}
