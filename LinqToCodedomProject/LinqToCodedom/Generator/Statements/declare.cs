using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using LinqToCodedom.Visitors;
using System.Linq.Expressions;

namespace LinqToCodedom.Generator
{
    public static partial class Builder
    {
        public static CodeVariableDeclarationStatement declare(CodeTypeReference type, string varName)
        {
            return new CodeVariableDeclarationStatement(type, varName, Builder.@default(type));
        }

        public static CodeVariableDeclarationStatement declare(string type, string varName)
        {
            return new CodeVariableDeclarationStatement(type, varName, Builder.@default(type));
        }

        public static CodeVariableDeclarationStatement declare(Type type, string varName)
        {
            return new CodeVariableDeclarationStatement(type, varName, Builder.@default(type));
        }
        
        public static CodeVariableDeclarationStatement declare<TResult>(
            string varName, Expression<Func<TResult>> initExp)
        {
            return new CodeVariableDeclarationStatement(typeof(TResult), varName,
                new CodeExpressionVisitor(new VisitorContext()).Visit(initExp));
        }

        public static CodeVariableDeclarationStatement declare<T, TResult>(
            string varName, Expression<Func<T, TResult>> initExp)
        {
            return new CodeVariableDeclarationStatement(typeof(TResult), varName,
                new CodeExpressionVisitor(new VisitorContext()).Visit(initExp));
        }

    }
}
