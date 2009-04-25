using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GenericObjectCreate()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CastExpression()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TypeofExpression()
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

    }
}
