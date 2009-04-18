using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Visitors
{
    public class CodeExpressionVisitor
    {
        private CodeStatementVisitor _vis;

        public CodeExpressionVisitor(CodeStatementVisitor vis)
        {
            _vis = vis;
        }

        public CodeExpression Visit(Expression exp)
        {
            if (exp == null)
                return null;

            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                //return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                //return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                //return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                //return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                //return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                //return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                //return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                //return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                //return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                //return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                //return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        private CodeExpression VisitConstant(ConstantExpression constantExpression)
        {
            return new CodePrimitiveExpression(constantExpression.Value);
        }

        private CodeExpression VisitParameter(ParameterExpression parameterExpression)
        {
            return _vis.Params.Find((p) => p.ParameterName == parameterExpression.Name);
        }

        private CodeExpression VisitMemberAccess(MemberExpression memberExpression)
        {
            var c = Visit(memberExpression.Expression);
            if (c is CodeSnippetExpression)
                throw new NotImplementedException();
            else
                return c;
        }
    }
}
