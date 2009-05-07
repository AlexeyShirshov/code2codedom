﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToCodedom;
using LinqToCodedom.Generator;
using System.CodeDom;
using LinqToCodedom.Extensions;

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
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (VarRef<string> s) => s),
                        Emit.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddMethod("TestClass", MemberAttributes.Static | MemberAttributes.Public, () => "Create",
                    Emit.@return(() => CodeDom.@new("TestClass", 100, "yyy"))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void GenericObjectCreate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
            .AddClass(Define.Class("TestClass").Generic("T")
                .AddFields(
                    Define.Field("T", MemberAttributes.Private, "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            ).AddClass(Define.Class("cls")
                .AddMethod(CodeDom.TypeRef("TestClass", "T"), MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare(CodeDom.TypeRef("TestClass", "T"), "cc",
                        () => CodeDom.@new(CodeDom.TypeRef("TestClass", "T"))),
                    Emit.@return((Var cc) => cc)
                ).Generic("T")
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

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
        public void StaticDelegate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public,
                    ()=>"foo",
                    Emit.declare(typeof(EventHandler), "h"),
                    Emit.assignDelegate("h", "zoo"),
                    Emit.stmt((Var h)=>h.Call()(null, null))
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Private,(object sender, EventArgs args)=>"zoo")                    
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void InstanceDelegate()
        {
            var c = new CodeDomGenerator();

            c.AddReference("System.Core.dll").AddNamespace("Samples").AddClass("cls")
                .AddMethod(typeof(string), MemberAttributes.Static | MemberAttributes.Public,
                    () => "foo",
                    Emit.declare("h2", () => new Func<string>("aaa".ToString)),
                    Emit.@return((VarRef<Func<string>> h2) => h2.v())
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);

            string s = (string)TestClass.InvokeMember("foo",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod,
                null, null, null);

            Assert.AreEqual("aaa", s);

        }

        [TestMethod]
        public void InstanceClassDelegate()
        {
            var c = new CodeDomGenerator();

            c.AddReference("System.Core.dll").AddNamespace("Samples").AddClass("cls")
                .AddMethod(typeof(string), MemberAttributes.Public,
                    () => "foo",
                    Emit.declare(typeof(Func<int, string>), "h2"),
                    Emit.assignDelegate("h2", CodeDom.@this, "zoo"),
                    Emit.@return((VarRef<Func<int, string>> h2) => h2.v(10))
                )
                .AddMethod(typeof(string), MemberAttributes.Public, (int i) => "zoo",
                    Emit.@return((VarRef<int> i)=>i.ToString())
                )
            .AddClass("cls2")
                .AddMethod(typeof(string), MemberAttributes.Public | MemberAttributes.Static, 
                    (DynType cc) => "foo"+cc.SetType("cls"),
                    Emit.declare(typeof(Func<int, string>), "h2"),
                    Emit.assignDelegate("h2", CodeDom.VarRef("cc"), "zoo"),
                    Emit.@return((VarRef<Func<int, string>> h2) => h2.v(100))
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type cls = ass.GetType("Samples.cls");

            Assert.IsNotNull(cls);

            object t = cls.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            string s = (string)cls.InvokeMember("foo",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance,
                null, t, null);

            Assert.AreEqual("10", s);

            Type cls2 = ass.GetType("Samples.cls2");

            Assert.IsNotNull(cls2);

            object t2 = cls2.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t2);

            string s2 = (string)cls2.InvokeMember("foo",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static,
                null, t2, new object[]{t});

            Assert.AreEqual("100", s2);
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
