using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToCodedom;
using LinqToCodedom.Visitors;
using System.CodeDom;
using LinqToCodedom.Generator;
using System.IO;

namespace LinqToCodedomTest
{
    /// <summary>
    /// Summary description for ConditionalStatementTest
    /// </summary>
    [TestClass]
    public class DeclarationTest
    {
        public DeclarationTest()
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
        public void Builder_MethodCall()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(
              c.Class("TestClass")
                .AddMethod(typeof(int), MemberAttributes.Public, (int a) => "Test",
                    Builder.@return((ParamRef<int> a) => a+100)
                )
                .AddMethod(
                    Builder.Method(typeof(int), MemberAttributes.Public, (int a) => "Test1",
                        Builder.@return(() => 
                            Builder.VarRef<int>("a") +
                            Builder.@this.Call<int>("Test").Args(() => Builder.Seq(3,4,5)))
                    )
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual(104, TestClass.InvokeMember("Test", System.Reflection.BindingFlags.Default, null, t,
                new object[] { 4 }));

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Default, null, t,
                new object[] { 1 }));

            Builder.@this.Call("dsfg", () => Builder.Seq(3, 4, 5));

            Builder.@this.Call("dsfg", () => Builder.NamedSeq(new { i = 10, s = ""}));
        }

        [TestMethod]
        public void Builder_GetProperty()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(
              c.Class("TestClass")
                .GetProperty(typeof(int), MemberAttributes.Public, "Test",
                    Builder.@return(() => 100)
                )
                .AddMethod(typeof(int), MemberAttributes.Public, (int a) => "Test1",
                    Builder.@return((ParamRef<int> a) =>
                        a + Builder.@this.Property<int>("Test"))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

        }

        [TestMethod]
        public void Builder_Property()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_GenericMethod()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_GenericClass()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_Delegate()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_Ctors()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_Attribute()
        {
            var c = new CodeDom();

            Builder.Attribute("string", () => new { i = 10, s = "sdf" });

            Assert.Inconclusive();
        }
    }
}
