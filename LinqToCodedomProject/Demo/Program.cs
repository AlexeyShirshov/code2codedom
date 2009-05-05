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

namespace Demo
{
    class Program
    {

        static void Main()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples")
            .AddClass(
              c.Class("TestClass")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int a)=>"Print",
                    Builder.stmt(() => Console.WriteLine("Hello, world!")),
                    Builder.stmt((ParamRef<int> a) => Console.WriteLine(a)),
                    Builder.@if((ParamRef<int> a) => a == 10,
                        Builder.stmt(() => Console.WriteLine("a equals 10")))
                    //Builder.ifelse((Par<int> a) => a.GetHashCode() == 10 && a < 1 && (2 + 3) < 7,
                    //    Builder.GetStmts(Builder.stmt(() => Console.WriteLine("true"))),
                    //    Builder.stmt(() => Console.WriteLine("false")))
                )
            );


            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDom.Language.VB));

            var method = c.Compile().GetType("Samples.TestClass").GetMethod("Print");

            Console.WriteLine("Program is compiled and prints");
            Console.WriteLine("-----------------------------");

            method.Invoke(null, new object[] {10});
        }

    }
}

