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
    public class ConditionalStatementTest
    {
        public ConditionalStatementTest()
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
        public void Builder_If_Processes_Simple_Expressions()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples")
                .AddClass(
                  c.Class("TestClass")
                    .AddMethod(
                        Builder.Method(null, MemberAttributes.Public | MemberAttributes.Static, (int a) => "Print",
                            Builder.If( (Par<int> a) => a.v == 0, Builder.stmt(() => Console.WriteLine("unit"))))));

            string code = c.GenerateCode(LinqToCodedom.CodeDom.Language.CSharp);
            Console.WriteLine(code);
            Assert.IsTrue(code.Length > 0);
            Assert.IsTrue(code.Contains("if"));

            c.Compile();
        }


    
    }
}
