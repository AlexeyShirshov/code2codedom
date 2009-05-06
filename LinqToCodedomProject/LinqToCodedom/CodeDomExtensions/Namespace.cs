using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LinqToCodedom.Generator;

namespace LinqToCodedom.Extensions
{
    public static class CodeNamespaceExtensions
    {
        public static CodeNamespace Imports(this CodeNamespace codeNamespace, string namespaceName)
        {
            codeNamespace.Imports.Add(new CodeNamespaceImport(namespaceName));

            return codeNamespace;
        }

        #region Class

        public static CodeNamespace AddClass(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }

        public static CodeNamespace AddClass(this CodeNamespace codeNamespace, CodeTypeMember member)
        {
            codeNamespace.Types.Add(member.GetDeclaration());

            return codeNamespace;
        }

        public static CodeTypeDeclaration AddClass(this CodeNamespace codeNamespace, string className)
        {
            var c = Define.Class(className);

            codeNamespace.Types.Add(c);

            return c;
        }

        public static CodeTypeDeclaration AddClass(this CodeNamespace codeNamespace, string className,
            MemberAttributes ma)
        {
            var c = Define.Class(className, ma);

            codeNamespace.Types.Add(c);

            return c;
        }

        #endregion

        #region Interface

        public static CodeNamespace AddInterface(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }

        public static CodeNamespace AddInterface(this CodeNamespace codeNamespace, CodeTypeMember member)
        {
            codeNamespace.Types.Add(member.GetDeclaration());

            return codeNamespace;
        }

        #endregion

        #region Struct

        public static CodeNamespace AddStruct(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }

        public static CodeNamespace AddStruct(this CodeNamespace codeNamespace, CodeTypeMember member)
        {
            codeNamespace.Types.Add(member.GetDeclaration());

            return codeNamespace;
        }

        #endregion

        #region Enum

        public static CodeNamespace AddEnum(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }

        public static CodeTypeDeclaration AddEnum(this CodeNamespace codeNamespace, string enumName)
        {
            var e = Define.Enum(enumName);

            codeNamespace.Types.Add(e);

            return e;
        }

        #endregion
    }
}
