using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToCodedom;
using LinqToCodedom.Generator;
using System.CodeDom;

namespace LinqToCodedomTest
{
    /// <summary>
    /// Summary description for ExpressionTest
    /// </summary>
    [TestClass]
    public class ExpressionTest
    {
        public ExpressionTest()
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
        public void ArrayCreate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ObjectCreate()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Builder.assignField("_s", (VarRef<string> s) => s),
                        Builder.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddMethod("TestClass", MemberAttributes.Static | MemberAttributes.Public, () => "Create",
                    Builder.@return(() => Builder.@new("TestClass", 100, "yyy"))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void GenericObjectCreate()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass").Generic("T")
                .AddFields(
                    Define.Field("T", MemberAttributes.Private, "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            ).AddClass(c.Class("cls")
                .AddMethod(Builder.TypeRef("TestClass", "T"), MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Builder.declare(Builder.TypeRef("TestClass", "T"), "cc",
                        () => Builder.@new(Builder.TypeRef("TestClass", "T"))),
                    Builder.@return((Var cc) => cc)
                ).Generic("T")
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass`1");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void CastExpression()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TypeOfExpression()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DefaultExpression()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DelegateCreate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void WorkingWithEvent()
        {
            //attach
            //dettach
            //raise
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ParamByRef()
        {
            Assert.Inconclusive();
        }
    }
}
