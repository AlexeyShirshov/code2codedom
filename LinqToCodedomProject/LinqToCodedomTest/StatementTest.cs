using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom;
using LinqToCodedom;
using LinqToCodedom.Generator;
using System.Linq.Expressions;

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
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test",
                        Emit.ifelse((ParamRef<int> a) => a.v == 10,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        )
                    )
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test2",
                        Emit.ifelse((ParamRef<int> a) => a.v < 10,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        )
                    )
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test3",
                        Emit.ifelse((ParamRef<int> a) => a.v * 3 < 7,
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        )
                    )
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test4",
                        Emit.ifelse((ParamRef<int> a) => Math.Abs(a.v) * 3 < 7 + Math.Min(4, a.v),
                            CodeDom.CombineStmts(Emit.@return(() => 1)),
                            Emit.@return(() => -1)
                        )
                    )
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
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test",
                        Emit.declare("res", () => 0),
                        Emit.@for(
                            "i", //int i
                            (ParamRef<int> a) => a,  // = a 
                            (VarRef<int> i) => i < 10, //i<10
                            (VarRef<int> i) => i + 1, //i+=1
                                Emit.assign((VarRef<int> res) => CodeDom.nil, (VarRef<int> res) => res + 1)
                        ),
                        Emit.@return((VarRef<int> res) => res)
                    )
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public | MemberAttributes.Static, (int a) => "Test1",
                        Emit.declare("res", () => 0),
                        Emit.@for("i", (ParamRef<int> a) => a, (VarRef<int> i) => i < 10, () => CodeDom.VarRef<int>("i") + 2,
                            Emit.assignVar("res", () => CodeDom.VarRef<int>("res") + 1)
                        ),
                        Emit.@return(() => CodeDom.VarRef<int>("res") + 100)
                    )
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
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestGoTo()
        {
            Assert.Inconclusive();
        }
    }
}
