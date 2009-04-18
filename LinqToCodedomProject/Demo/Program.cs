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
                    .AddMethod(
                        Builder.Method(null, MemberAttributes.Public | MemberAttributes.Static, (int a)=>"Print",
                            Builder.stmt((Par<int> a) => Console.WriteLine(a.v)),
                            Builder.stmt(() => Console.WriteLine("Hello, world!"))
                        )
                )
            );

            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDom.Language.VB));

            var method = c.Compile().GetType("Samples.TestClass").GetMethod("Print");

            Console.WriteLine("Program is compiled and prints");

            method.Invoke(null, new object[] {10});
        }

    }
}

