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

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, Type baseType)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.Add(baseType);

            return c;
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, string baseType)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.Add(baseType);

            return c;
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, params Type[] interfaces)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.AddRange(interfaces.Select((t) => new CodeTypeReference(t)).ToArray());

            return c;
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, params string[] interfaces)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.AddRange(interfaces.Select((t) => new CodeTypeReference(t)).ToArray());

            return c;
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, Type baseType, params Type[] interfaces)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.Add(baseType);
            c.BaseTypes.AddRange(interfaces.Select((t) => new CodeTypeReference(t)).ToArray());

            return c;
        }

        public CodeTypeDeclaration Class(string className, MemberAttributes attributes, string baseType, params string[] interfaces)
        {
            var c = new CodeTypeDeclaration(className)
            {
                Attributes = attributes,
            };

            c.BaseTypes.Add(baseType);
            c.BaseTypes.AddRange(interfaces.Select((t) => new CodeTypeReference(t)).ToArray());

            return c;
        }

        #endregion

    }
}
