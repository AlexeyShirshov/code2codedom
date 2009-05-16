using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using LinqToCodedom;
using LinqToCodedom.Visitors;
using LinqToCodedom.Generator;
using LinqToCodedom.Extensions;

namespace Demo
{
    class Program
    {

        static void Main()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
            .AddClass("TestClass")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int a) => "Print",
                    Emit.stmt(() => Console.WriteLine("Hello, world!")),
                    Emit.stmt((int a) => Console.WriteLine(a)),
                    Emit.@if((int a) => a == 10,
                        Emit.stmt(() => Console.WriteLine("a equals 10")))
                //Builder.ifelse((Par<int> a) => a.GetHashCode() == 10 && a < 1 && (2 + 3) < 7,
                //    Builder.GetStmts(Builder.stmt(() => Console.WriteLine("true"))),
                //    Builder.stmt(() => Console.WriteLine("false")))
            );


            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.VB));

            var method = c.Compile().GetType("Samples.TestClass").GetMethod("Print");

            Console.WriteLine("Program is compiled and prints");
            Console.WriteLine("-----------------------------");

            method.Invoke(null, new object[] { 10 });
        }

        static void Main2()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), (int i) => "Print",
                   Emit.@if((int i) => i < 10,
                      Emit.@return(() => "i less than 10")
                   ),
                   Emit.@return(() => "i greater than 10")
                );

            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));

            var c2 = new CodeDomGenerator();
            
            CodeMemberMethod method = Define.Method(MemberAttributes.Public, () => "foo");

            c2.AddNamespace("TestNS").AddClass("Fibonacci")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int x) => "Calc",
                    Emit.@if((int x) => x <= 1, 
                        Emit.@return(()=>1)),
                    Emit.@return((int x) => 
                        CodeDom.Call<int>("Calc")(x - 1) + CodeDom.Call<int>("Calc")(x - 2))
                )
            ;

            Console.WriteLine(c2.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c2.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.VB));
        }

        static void Main3()
        {
            CodeMemberMethod Calc = new CodeMemberMethod()
            {
                Name = "Calc",
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };

            Calc.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "x"));

            Calc.Statements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.LessThanOrEqual,
                        new CodeSnippetExpression("1")
                    ),
                    new CodeMethodReturnStatement(new CodeSnippetExpression("1"))
                )
            );

            Calc.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeMethodInvokeExpression(null, "Calc", 
                            new CodeBinaryOperatorExpression(
                                new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.Subtract,
                                new CodeSnippetExpression("1")
                            )
                        ), CodeBinaryOperatorType.Add,
                        new CodeMethodInvokeExpression(null, "Calc", 
                            new CodeBinaryOperatorExpression(
                                new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.Subtract,
                                new CodeSnippetExpression("2")
                            )
                        )
                    )
                )
            );

            CodeTypeDeclaration Fibonacci = new CodeTypeDeclaration();
            Fibonacci.Members.Add(Calc);

            CodeNamespace ns = new CodeNamespace("TestNS");
            ns.Types.Add(Fibonacci);

            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(ns);

            using (TextWriter tw = new IndentedTextWriter(Console.Out))
            {
                using (CodeDomProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider())
                {
                    codeProvider.GenerateCodeFromCompileUnit(unit, tw, new CodeGeneratorOptions());
                }
            }
        }
    }
}

