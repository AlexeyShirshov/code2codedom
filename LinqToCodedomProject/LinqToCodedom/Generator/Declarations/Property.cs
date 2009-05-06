using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace LinqToCodedom.Generator
{
    public static partial class Define
    {
        public static CodeMemberProperty GetProperty(Type propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
                HasGet = true
            };

            if (statements != null)
                c.GetStatements.AddRange(statements);

            return c;
        }

        public static CodeMemberProperty GetProperty(string propertyType, MemberAttributes ma, string name,
            params CodeStatement[] statements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
                HasGet = true
            };

            if (statements != null)
                c.GetStatements.AddRange(statements);

            return c;
        }

        public static CodeMemberProperty GetProperty(Type propertyType, MemberAttributes ma, string name,
            string fieldName)
        {
            return GetProperty(propertyType, ma, name,
                Emit.@return(() => CodeDom.@this.Field<object>(fieldName)));
        }

        public static CodeMemberProperty GetProperty(string propertyType, MemberAttributes ma, string name,
            string fieldName)
        {
            return GetProperty(propertyType, ma, name,
                Emit.@return(() => CodeDom.@this.Field<object>(fieldName)));
        }

        public static CodeMemberProperty Property(Type propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
                HasGet = true,
                HasSet = true
            };

            if (getStatements != null)
                c.GetStatements.AddRange(getStatements);

            if (setStatements != null)
                c.SetStatements.AddRange(setStatements);

            return c;
        }

        public static CodeMemberProperty Property(Type propertyType, MemberAttributes ma, string name,
            string fieldName)
        {
            return Property(propertyType,ma, name,
                CodeDom.CombineStmts(Emit.@return(() => CodeDom.@this.Field<object>(fieldName))),
                Emit.assignField(fieldName, (SetValueRef<object> value) => value));
        }

        public static CodeMemberProperty Property(Type propertyType, MemberAttributes ma, string name)
        {
            return Property(propertyType, ma, name, null, null);
        }

        public static CodeMemberProperty Property(string propertyType, MemberAttributes ma, string name)
        {
            return Property(propertyType, ma, name, null, null);
        }

        public static CodeMemberProperty Property(string propertyType, MemberAttributes ma, string name,
            CodeStatement[] getStatements, params CodeStatement[] setStatements)
        {
            var c = new CodeMemberProperty()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(propertyType),
                HasGet = true,
                HasSet = true
            };

            if (getStatements != null)
                c.GetStatements.AddRange(getStatements);

            if (setStatements != null)
                c.SetStatements.AddRange(setStatements);

            return c;
        }

        public static CodeMemberProperty Property(string propertyType, MemberAttributes ma, string name,
            string fieldName)
        {
            return Property(propertyType, ma, name,
                CodeDom.CombineStmts(Emit.@return(() => CodeDom.@this.Field<object>(fieldName))),
                Emit.assignField(fieldName, (SetValueRef<object> value) => value));
        }

    }
}
