using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;
using LinqToCodedom.Visitors;

namespace LinqToCodedom.Generator
{
    public static partial class Define
    {
        public static CodeMemberField Field(Type fieldType, MemberAttributes ma, string name)
        {
            return new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
            };
        }

        public static CodeMemberField Field(CodeTypeReference fieldType, MemberAttributes ma, string name)
        {
            return new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = fieldType,
            };
        }

        public static CodeMemberField Field<T, TResult>(Type fieldType, MemberAttributes ma, string name,
            Expression<Func<T, TResult>> exp)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
                InitExpression = new CodeExpressionVisitor(new VisitorContext()).Visit(exp),
            };

            return c;
        }

        public static CodeMemberField Field<TResult>(Type fieldType, MemberAttributes ma, string name,
            Expression<Func<TResult>> exp)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
                InitExpression = new CodeExpressionVisitor(new VisitorContext()).Visit(exp),
            };

            return c;
        }

        public static CodeMemberField Field(string fieldType, MemberAttributes ma, string name)
        {
            return new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
            };
        }

        public static CodeMemberField Field<T, TResult>(string fieldType, MemberAttributes ma, string name,
            Expression<Func<T, TResult>> exp)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
                InitExpression = new CodeExpressionVisitor(new VisitorContext()).Visit(exp),
            };

            return c;
        }

        public static CodeMemberField Field<TResult>(string fieldType, MemberAttributes ma, string name,
            Expression<Func<TResult>> exp)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = ma,
                Type = new CodeTypeReference(fieldType),
                InitExpression = new CodeExpressionVisitor(new VisitorContext()).Visit(exp),
            };

            return c;
        }

        public static CodeMemberField Field<TResult>(string name,
            Expression<Func<TResult>> exp)
        {
            var c = new CodeMemberField()
            {
                Name = name,
                Attributes = MemberAttributes.Private,
                Type = new CodeTypeReference(typeof(TResult)),
                InitExpression = new CodeExpressionVisitor(new VisitorContext()).Visit(exp),
            };

            return c;
        }
    }
}
