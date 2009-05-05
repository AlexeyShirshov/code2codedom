using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom
{
    public partial class CodeDom
    {
        #region Class declaration

        public CodeTypeDeclaration Class(string className)
        {
            return new CodeTypeDeclaration(className);
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes)
        {
            return new CodeTypeDeclaration(className)
            {
                Attributes = attributes
            };
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, bool partial)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
                IsPartial = partial
            };

            return c;
        }

        public CodeTypeDeclaration Struct(string structName)
        {
            return new CodeTypeDeclaration(structName) { IsStruct = true };
        }

        public CodeTypeDeclaration Struct(string structName, MemberAttributes attributes)
        {
            return new CodeTypeDeclaration(structName)
            {
                Attributes = attributes,
                IsStruct = true
            };
        }

        public CodeTypeDeclaration Struct(string structName, MemberAttributes attributes, bool partial)
        {
            var c = new CodeTypeDeclaration(structName)
            {
                Attributes = attributes,
                IsPartial = partial,
                IsStruct = true
            };

            return c;
        }

        public CodeTypeDeclaration Interface(string interfaceName)
        {
            return new CodeTypeDeclaration(interfaceName) { IsInterface = true };
        }

        public CodeTypeDeclaration Interface(string interfaceName, MemberAttributes attributes)
        {
            return new CodeTypeDeclaration(interfaceName)
            {
                Attributes = attributes,
                IsInterface = true
            };
        }

        public CodeTypeDeclaration Enum(string enumName)
        {
            return new CodeTypeDeclaration(enumName) { IsEnum = true };
        }

        public CodeTypeDeclaration Enum(string enumName, MemberAttributes attributes)
        {
            return new CodeTypeDeclaration(enumName)
            {
                Attributes = attributes,
                IsEnum = true
            };
        }
        #endregion

    }
}
