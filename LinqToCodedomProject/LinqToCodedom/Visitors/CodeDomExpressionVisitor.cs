using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace ExpressionToCodedom
{
    public  class CodeDomExpressionVisitor
    {

        Expression m_exp;
        Dictionary<string, CodeTypeMember> m_members;

        public CodeDomExpressionVisitor(Expression e)
        {
            m_exp = e;
        }
        internal string GenerateSource(CodeDomProvider codeProvider)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tWriter = new IndentedTextWriter(new StringWriter(sb));
            CodeCompileUnit ccu = GenerateCode();
            codeProvider.GenerateCodeFromCompileUnit(ccu, tWriter, new CodeGeneratorOptions());
            codeProvider.Dispose();

            tWriter.Close();

            return sb.ToString();
        }

        internal string GenerateSource(string language)
        {
            

            CodeDomProvider codeProvider=null;
            if (language == "cs")
                codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            else if (language == "vb")
                codeProvider = new Microsoft.VisualBasic.VBCodeProvider();
            else
            {                
                
                    throw new Exception("make sure you are trying to load a CodeDomProvider assembly");
                
            }
            return GenerateSource(codeProvider); 
           
        }

        public string GenerateSource()
        {
            return GenerateSource("cs"); 
        }


        private CodeCompileUnit GenerateCode()
        {
            var code = new CodeCompileUnit();
            m_members = new Dictionary<string, CodeTypeMember>();

            var LambdaTypeClass = new CodeTypeDeclaration("LambdaExpression");
            var ns = new CodeNamespace("Runtime");
            
            ns.Types.Add(LambdaTypeClass);            
            ns.Imports.Add(new CodeNamespaceImport("System"));
            // add more types in case I want to compile

            code.Namespaces.Add(ns);

            CodeObject cEvaluationResult = Visit(m_exp);

            var constructor = new CodeConstructor();

            if (cEvaluationResult is CodeStatement)
                constructor.Statements.Add(cEvaluationResult as CodeStatement);
            
            else if (cEvaluationResult is CodeExpression)
                constructor.Statements.Add(cEvaluationResult as CodeExpression);

            LambdaTypeClass.Members.Add(constructor);
            

            foreach (var item in m_members)
            {
                LambdaTypeClass.Members.Add(item.Value);
            }

            return code;

        }

        protected virtual CodeObject Visit(Expression exp)
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
                    return this.VisitUnary((UnaryExpression)exp);
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
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        protected virtual CodeObject VisitBinding(MemberBinding binding)
        {            
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        protected virtual CodeExpression VisitElementInitializer(ElementInit initializer)
        {            
            ReadOnlyCollection<CodeExpression> arguments = this.VisitExpressionList(initializer.Arguments);

            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeThisReferenceExpression(),initializer.AddMethod.Name), arguments.ToArray());                               
        }

        protected virtual CodeObject VisitUnary(UnaryExpression u)
        {
            CodeObject operand = this.Visit(u.Operand);
          
            return operand;
        }

        private CodeBinaryOperatorType BindOperant(ExpressionType e)
        {
            switch (e)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return CodeBinaryOperatorType.Add;
                   
                case ExpressionType.And:
                    return CodeBinaryOperatorType.BitwiseAnd;
                    
                case ExpressionType.AndAlso:
                    return CodeBinaryOperatorType.BooleanAnd;                  

                case ExpressionType.Or:
                    return CodeBinaryOperatorType.BitwiseOr;                    

                case ExpressionType.OrElse:
                    return CodeBinaryOperatorType.BooleanOr;                    

                case ExpressionType.ExclusiveOr:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                    throw new NotSupportedException("no direct equivalent in codedom,so workarounds not implemented");

                case ExpressionType.Equal:
                    return CodeBinaryOperatorType.IdentityEquality;
                    
                case ExpressionType.NotEqual:
                    return CodeBinaryOperatorType.IdentityInequality;                    

                case ExpressionType.GreaterThan:
                    return CodeBinaryOperatorType.GreaterThan;                    

                case ExpressionType.GreaterThanOrEqual:
                    return CodeBinaryOperatorType.GreaterThanOrEqual;                    

                case ExpressionType.LessThan:
                    return CodeBinaryOperatorType.LessThan;                    

                case ExpressionType.LessThanOrEqual:
                    return CodeBinaryOperatorType.LessThanOrEqual;                    

                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return CodeBinaryOperatorType.Multiply;
                    
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return CodeBinaryOperatorType.Subtract;

                case ExpressionType.Power:
                case ExpressionType.Divide:
                    return CodeBinaryOperatorType.Divide;
                    
                case ExpressionType.Modulo:
                    return CodeBinaryOperatorType.Modulus;
                    
                default:
                    throw new Exception("are you sure you are right?");
            }
        }

        protected virtual CodeBinaryOperatorExpression VisitBinary(BinaryExpression b)
        {
            var left = this.Visit(b.Left) as CodeExpression;
            var right = this.Visit(b.Right) as CodeExpression;
            CodeObject conversion = this.Visit(b.Conversion);

            CodeBinaryOperatorType operant = BindOperant(b.NodeType);           
            var condExpr = new CodeBinaryOperatorExpression(left, operant, right);
            return condExpr;
        }

        protected virtual CodeObject VisitTypeIs(TypeBinaryExpression b)
        {            
            CodeObject expr = this.Visit(b.Expression);          
            return expr;
        }

        protected virtual CodeExpression VisitConstant(ConstantExpression c)
        {
            if (c.Value == null)
            {
                return new CodePrimitiveExpression(null);
            }
            else if (c.Value.GetType().IsValueType || c.Value.GetType() == typeof(string))
            {
                   return new CodePrimitiveExpression(c.Value);
            }
            else
            {
                return new CodeVariableReferenceExpression(c.Value.ToString());             
            }                        
        }

        protected virtual CodeObject VisitConditional(ConditionalExpression c)
        {            
            CodeObject test = this.Visit(c.Test);
            CodeExpression ifTrue = this.Visit(c.IfTrue) as CodeExpression;
            CodeExpression ifFalse = this.Visit(c.IfFalse) as CodeExpression;

            var ifStatement = new CodeConditionStatement(test as CodeExpression,
                                                         new CodeStatement[] {new CodeExpressionStatement(ifTrue) }, 
                                                         new CodeStatement[] {new CodeExpressionStatement(ifFalse) });                    
            return ifStatement;
        }

        protected virtual CodeObject VisitParameter(ParameterExpression p)
        {
            return new CodeArgumentReferenceExpression(p.Name);            
        }

        protected virtual CodeObject VisitMemberAccess(MemberExpression m)
        {

            CodeObject exp = this.Visit(m.Expression);

            if (exp is CodePrimitiveExpression)
            {
                return exp;
            }
            else
            {
                Type memType;
                if (m.Member.MemberType == MemberTypes.Field)
                    memType = (m.Member as FieldInfo).FieldType;
                else memType = (m.Member as PropertyInfo).PropertyType;


                m_members[m.Member.Name] = new CodeMemberField(memType, m.Member.Name);
                return new CodeVariableReferenceExpression(m.Member.Name);
            }
        }    

        protected virtual CodeObject VisitMethodCall(MethodCallExpression m)
        {           
            CodeObject obj = this.Visit(m.Object);
            IEnumerable<CodeExpression> args = this.VisitExpressionList(m.Arguments);
  
            if (obj == null)
            {  //static method call
                return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(m.Method.DeclaringType),m.Method.Name,args.ToArray());                
            }
            else
            {
                return new CodeMethodInvokeExpression(obj as CodeExpression, m.Method.Name, args.ToArray());
            }   
        }

        protected virtual ReadOnlyCollection<CodeExpression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                CodeExpression p = (CodeExpression)this.Visit(original[i]);                
                    list.Add(p);                
            }            
            return list.AsReadOnly();
        }

        protected virtual CodeExpression VisitMemberAssignment(MemberAssignment assignment)
        {// thhose are properties
           
            CodeObject e = this.Visit(assignment.Expression);
            return e as CodeExpression;
           
        }

        protected virtual CodeObject VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            
            IEnumerable<CodeExpression> bindings = this.VisitBindingList(binding.Bindings) as IEnumerable<CodeExpression>;
            return new CodeObjectCreateExpression(binding.Member.Name, bindings.ToArray());            
        }

        protected virtual CodeObject VisitMemberListBinding(MemberListBinding binding)
        {
            
            IEnumerable<CodeExpression> initializers = this.VisitElementInitializerList(binding.Initializers);

            return new CodeObjectCreateExpression(binding.Member.Name, initializers.ToArray());
            
        }

        protected virtual IEnumerable<CodeExpression> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                CodeExpression b = this.VisitBinding(original[i]) as CodeExpression;
                
                    list.Add(b);
               
            }
            return list;
        }

        protected virtual IEnumerable<CodeExpression> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<CodeExpression> list = new List<CodeExpression>();
            for (int i = 0, n = original.Count; i < n; i++)
            {
                CodeExpression init = this.VisitElementInitializer(original[i]);
                
                list.Add(init);
                              
            }
            
            return list;
        }

        protected CodeMethodReferenceExpression VisitLambda(LambdaExpression lambda)
        {
            var  body = this.Visit(lambda.Body);
            var lambdaMethod = new CodeMemberMethod();

            lambdaMethod.Name = lambda.Type.Name;
            if (lambdaMethod.Name.Contains("Func"))
                lambdaMethod.ReturnType = new CodeTypeReference(lambda.Body.Type);

            foreach (var item in lambda.Parameters)
            {
                lambdaMethod.Parameters.Add(new CodeParameterDeclarationExpression(item.Type, item.Name));
            }

            if (body is CodeExpression)
            {
                if (lambdaMethod.ReturnType.BaseType.Contains("Void"))
                    lambdaMethod.Statements.Add((body as CodeExpression ));

                else
                    lambdaMethod.Statements.Add(new CodeMethodReturnStatement(body as CodeExpression));
            }
            else if (body is CodeStatement)
            {
                    lambdaMethod.Statements.Add((body as CodeStatement));
            }
            else
            {
                throw new Exception("investigate...");
            }

            m_members[lambda.Type.FullName] = lambdaMethod;
            return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), lambdaMethod.Name) ;
        }

        protected virtual CodeObject VisitNew(NewExpression nex)
        {            
            IEnumerable<CodeExpression> args = this.VisitExpressionList(nex.Arguments);
         
            
            return new CodeObjectCreateExpression(nex.Type.Name,args.ToArray());
            
        }

        protected virtual CodeObject VisitMemberInit(MemberInitExpression init)
        {
            CodeObject n = this.VisitNew(init.NewExpression);
            CodeExpression[] bindings = this.VisitBindingList(init.Bindings).ToArray(); //binding will return property initialisation


            for (int i = 0; i < init.Bindings.Count; i++)            
            {
                                                                    // need to do something with that////
                var assignProperty = new CodeAssignStatement(new CodePropertyReferenceExpression(
                            n as CodeExpression, init.Bindings[i].Member.Name), bindings[i]);
            }                                   

            return n;
        }

        protected virtual CodeObject VisitListInit(ListInitExpression init)
        {
            
            CodeObject n = this.VisitNew(init.NewExpression);
            IEnumerable<CodeExpression> initializers = this.VisitElementInitializerList(init.Initializers);
          
            return n;
        }

        protected virtual CodeObject VisitNewArray(NewArrayExpression na)
        {

            IEnumerable<CodeExpression> exprs = this.VisitExpressionList(na.Expressions);

                                                 
            return new CodeArrayCreateExpression(new CodeTypeReference(na.Type), exprs.ToArray());
        }

        protected virtual CodeObject VisitInvocation(InvocationExpression iv)
        {            
            IEnumerable<CodeExpression> args = this.VisitExpressionList(iv.Arguments);

            var expr = this.Visit(iv.Expression) as CodeExpression;

            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(expr, "Method"), args.ToArray());            
        }      
    }
}