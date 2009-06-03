using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom;
using LinqToCodedom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;
using LinqToCodedom.Extensions;

namespace LinqToCodedomTest
{
    /// <summary>
    /// Summary description for StatementTest
    /// </summary>
    [TestClass]
    public class StatementTest
    {
        public StatementTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Builder_If()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test",
                        Emit.ifelse((int a) => a == 10,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        ))
                )
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test2",
                        Emit.ifelse((int a) => a < 10,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        ))
                )
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test3",
                        Emit.ifelse((int a) => a * 3 < 7,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        ))
                )
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test4",
                        Emit.ifelse((int a) => Math.Abs(a) * 3 < 7 + Math.Min(4, a),
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        ))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            Assert.AreEqual(1, c.Compile().GetType("Samples.TestClass").GetMethod("Test")
                .Invoke(null, new object[] { 10 }));

            Assert.AreEqual(-1, c.Compile().GetType("Samples.TestClass").GetMethod("Test")
                .Invoke(null, new object[] { 100 }));

            Assert.AreEqual(1, c.Compile().GetType("Samples.TestClass").GetMethod("Test2")
                .Invoke(null, new object[] { 7 }));

            Assert.AreEqual(-1, c.Compile().GetType("Samples.TestClass").GetMethod("Test2")
                .Invoke(null, new object[] { 100 }));

            Assert.AreEqual(-1, c.Compile().GetType("Samples.TestClass").GetMethod("Test3")
                .Invoke(null, new object[] { 10 }));

            Assert.AreEqual(1, c.Compile().GetType("Samples.TestClass").GetMethod("Test3")
                .Invoke(null, new object[] { 2 }));

            Assert.AreEqual(1, c.Compile().GetType("Samples.TestClass").GetMethod("Test4")
                .Invoke(null, new object[] { 2 }));

            Assert.AreEqual(-1, c.Compile().GetType("Samples.TestClass").GetMethod("Test4")
                .Invoke(null, new object[] { -10 }));
        }

        [TestMethod]
        public void Builder_Loop()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test",
                        Emit.declare("res", () => 0),
                        Emit.@for(
                            "i", //int i
                            (int a) => a,  // = a 
                            (int i) => i < 10, //i<10
                            (int i) => i + 1, //i+=1
                                Emit.assignVar("res", (int res) => res + 1)
                        ), Emit.@return((int res) => res))
                )
                .AddMethod(
                    Define.Method(MemberAttributes.Public | MemberAttributes.Static, typeof(int), (int a) => "Test1",
                        Emit.declare("res", () => 0),
                        Emit.@for("i", (int a) => a, (int i) => i < 10, () => CodeDom.VarRef<int>("i") + 2,
                            Emit.assignVar("res", () => CodeDom.VarRef<int>("res") + 1)
                        ), Emit.@return(() => CodeDom.VarRef<int>("res") + 100))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            Assert.AreEqual(5, c.Compile().GetType("Samples.TestClass").GetMethod("Test")
                .Invoke(null, new object[] { 5 }));

            Assert.AreEqual(103, c.Compile().GetType("Samples.TestClass").GetMethod("Test1")
                .Invoke(null, new object[] { 5 }));

        }

        [TestMethod]
        public void TestExceptionHandling()
        {
            //try catch
            //try finally
            //throw
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), () => "foo",
                    Emit.trycatch(Emit.@throw(() => new ApplicationException()))
                        .AddCatch(typeof(ApplicationException), "ex",
                            Emit.@return(() => "ok")
                        )
                        .AddFinally(Emit.stmt(() => Console.WriteLine())))
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestGoTo()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, typeof(int), () => "foo",
                    Emit.declare(typeof(int), "i"),
                    Emit.@goto("x"),
                    Emit.assignVar("i", () => 10),
                    Emit.label("x"),
                    Emit.@return((int i) => i))
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual(0, TestClass.InvokeMember("foo", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod, null, t, null));

        }

        [TestMethod]
        public void TestUsing()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@using((System.IO.MemoryStream ms) => ms,
                        Emit.stmt(() => Console.WriteLine("using"))
                    )
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "zoo",
                    Emit.@using("ms", () => new System.IO.MemoryStream(),
                        Emit.stmt(() => Console.WriteLine("using"))
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestLock()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@lock((System.IO.MemoryStream ms) => ms,
                        Emit.stmt(() => Console.WriteLine("using"))
                    )
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "zoo",
                    Emit.@lock(() => string.Intern("asdflaskj"),
                        Emit.stmt(() => Console.WriteLine("using"))
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestForeach()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@foreach("ch", () => "afdgfad".ToCharArray(),
                        Emit.stmt((char ch) => Console.WriteLine(ch))
                    )
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "zoo",
                    Emit.@foreach(new CodeTypeReference(typeof(object)), "ch", 
                        () => "afdgfad".ToCharArray(),
                        Emit.stmt((char ch) => Console.WriteLine(ch)),
                        Emit.continueFor()
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestBreak()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@foreach("ch", () => "afdgfad".ToCharArray(),
                        Emit.stmt((char ch) => Console.WriteLine(ch)),
                        Emit.exitFor()
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestDoBreak()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@do(() => true,
                        Emit.exitDo()
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestWhileBreak()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (System.IO.MemoryStream ms) => "foo",
                    Emit.@while(() => false,
                        Emit.exitWhile()
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestSwitch()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (int i) => "foo",
                    Emit.@switch((int i) => i)
                        .Case(1, 
                            Emit.stmt(()=>Console.WriteLine("1")),
                            Emit.exitSwitch()
                        )
                        .Case(2, 
                            Emit.stmt(() => Console.WriteLine("2")),
                            Emit.exitSwitch()
                        )
                        .CaseElse(
                            Emit.stmt((int i) => Console.WriteLine(i)),
                            Emit.exitSwitch()
                        )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }
    }
}
