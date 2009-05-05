using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LinqToCodedom
{
    public static class CodeNamespaceExtensions
    {
        public static CodeObject Clone(this CodeObject codeNamespace)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(ms, codeNamespace);
                ms.Seek(0, SeekOrigin.Begin);
                return f.Deserialize(ms) as CodeObject;
            }
        }

        public static CodeNamespace Imports(this CodeNamespace codeNamespace, string namespaceName)
        {
            codeNamespace.Imports.Add(new CodeNamespaceImport(namespaceName));

            return codeNamespace;
        }

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

        public static CodeNamespace AddStruct(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }

        public static CodeNamespace AddEnum(this CodeNamespace codeNamespace, CodeTypeDeclaration codeType)
        {
            codeNamespace.Types.Add(codeType);

            return codeNamespace;
        }
    }
}
