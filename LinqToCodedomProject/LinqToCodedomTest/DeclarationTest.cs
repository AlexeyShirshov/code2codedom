using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToCodedom;
using LinqToCodedom.Visitors;
using System.CodeDom;
using LinqToCodedom.Generator;
using LinqToCodedom.Extensions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LinqToCodedom.CodeDomPatterns;
using System.Reflection;

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
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddMethod(MemberAttributes.Public, typeof(int), (int a) => "Test", Emit.@return((int a) => a + 100))
                .AddMethod(
                    Define.Method(MemberAttributes.Public, typeof(int), (int a) => "Test1", Emit.@return(() =>
                            CodeDom.VarRef<int>("a") +
                            CodeDom.@this.Call<int>("Test")(3)))
                )
            ).AddClass(Define.Class("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("TestClass", "cc"),
                    Emit.stmt(() => CodeDom.Call(CodeDom.VarRef("cc"), "Test1")(3))
                )
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo2",
                    Emit.declare("TestClass", "cc", () => CodeDom.@new("TestClass")),
                    Emit.stmt((Var cc) => cc.Call("Test1")(3))
                )
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int s) => "foo3",
                    Emit.declare("TestClass", "cc"),
                    Emit.declare(typeof(int), "i"),
                    Emit.stmt((int s) => CodeDom.Call(CodeDom.VarRef("cc"), "Test1")(CodeDom.VarRef<int>("i")+s))
                )
                .AddMethod(MemberAttributes.Public, () => "foo4",
                    Emit.@foreach("mi", ()=>CodeDom.@this.Call<Type>("GetType")().GetMethods(BindingFlags.NonPublic | BindingFlags.Static),
                        Emit.stmt(() => CodeDom.VarRef("mi").Call("GetParameters")),
                        Emit.@if((MethodInfo mi) => mi.Name == "dfdfd",
                            Emit.@return()
                        ),
                        Emit.@return()
                    )
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual(104, TestClass.InvokeMember("Test", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }));

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 1 }));
        }

        [TestMethod]
        public void Builder_GetProperty()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "Test",
                    Emit.@return(() => 100)
                )
                .AddMethod(MemberAttributes.Public, typeof(int), (int a) => "Test1", Emit.@return((int a) =>
                        a + CodeDom.@this.Property<int>("Test")))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }));

        }

        [TestMethod]
        public void Builder_Property()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddFields(
                    Define.Field(MemberAttributes.Private, typeof(string), "_s")
                )
                .AddProperty(
                    Define.Property(typeof(string), MemberAttributes.Public, "Test",
                       CodeDom.CombineStmts(Emit.@return(() => CodeDom.@this.Field<string>("_s"))),
                       Emit.assignField("_s", (string value) => value)
                    )
                )
                .AddProperty(
                    Define.Property(typeof(string), MemberAttributes.Public, "Test2", "_s")
                )
                .AddProperty(typeof(string), MemberAttributes.Public, "Test3", "_s")
                .AddProperty("TestClass", MemberAttributes.Public, "Test4",
                   CodeDom.CombineStmts(Emit.@return(() => CodeDom.@this)),
                   Emit.@throw(() => new NotImplementedException(
                       CodeDom.Property<string>(CodeDom.VarRef("value"), "Test")
                   ))
                )
                .AddMethod(MemberAttributes.Public, typeof(string), (int a) => "Test1", 
                    Emit.assignProperty("Test", () => Guid.NewGuid().ToString()), 
                    Emit.@return((int a) =>
                        a.ToString() + CodeDom.@this.Property<string>("Test")))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            string s = TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }) as string;

            Assert.IsNotNull(s);

            string TestProperty = s.Substring(1);

            Assert.AreEqual(TestProperty, TestClass.InvokeMember("Test", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
        }

        [TestMethod]
        public void Builder_AccessProperty()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls1")
                .AddField(typeof(int), "_i", () => 10)
                .AddProperty(typeof(int), MemberAttributes.Public, "I", "_i")
            .AddClass("cls2")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(int), () => "foo", 
                    Emit.declare("cls1", "cc", () => CodeDom.@new("cls1")), 
                    Emit.@return((Var cc) => cc.Property<int>("I")))
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls2");

            Assert.IsNotNull(TestClass);

            int s = (int)TestClass.InvokeMember("foo", 
                BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, 
                null, null, null);

            Assert.AreEqual(10, s);
        }

        [TestMethod]
        public void Builder_GenericMethod()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass").Generic("T")
                .AddFields(
                    Define.Field(MemberAttributes.Private, "T", "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            ).AddClass(Define.Class("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, CodeDom.TypeRef("TestClass", "T"), () => "foo", 
                    Emit.declare(CodeDom.TypeRef("TestClass", "T"), "cc",
                        () => CodeDom.@new(CodeDom.TypeRef("TestClass", "T"))), 
                    Emit.@return((Var cc) => cc))
                .Generic("T")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, ()=>"foo2",
                    Emit.stmt(()=>CodeDom.Call(CodeDom.TypeRef("cls"), "foo", typeof(int)))
                )
                .AddMethod(MemberAttributes.Static, ()=>"foo3", 
                    Emit.@return()
                ).Generic("T", true, typeof(object))
                .AddMethod(MemberAttributes.Static, () => "foo5",
                    Emit.@return()
                ).Generic("T", true, "System.Object")
                .AddMethod(MemberAttributes.Static, () => "foo6",
                    Emit.@return()
                ).Generic("T", true, new CodeTypeReference(typeof(object)))
                .AddMethod(MemberAttributes.Static, () => "foo4",
                    Emit.@return()
                ).Generic("T", typeof(ValueType))
                .AddMethod(MemberAttributes.Static, () => "foo7",
                    Emit.@return()
                ).Generic("T", new CodeTypeReference(typeof(ValueType)))
                .AddMethod(MemberAttributes.Static, () => "foo8",
                    Emit.@return()
                ).Generic("T", "cls")
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass`1");

            Assert.IsNotNull(TestClass);

            Type realType = TestClass.MakeGenericType(new Type[] { typeof(string) });

            Assert.IsNotNull(realType);

            Type clsType = ass.GetType("Samples.cls");

            Assert.IsNotNull(clsType);

            System.Reflection.MethodInfo mi = clsType.GetMethod("foo");

            Assert.IsNotNull(mi);

            System.Reflection.MethodInfo rmi = mi.MakeGenericMethod(typeof(string));

            object t = rmi.Invoke(null, null);

            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void Builder_GenericClass()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .Generic("T")
                .AddFields(
                    Define.Field(MemberAttributes.Private, "T", "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass`1");

            Assert.IsNotNull(TestClass);

            Type realType = TestClass.MakeGenericType(new Type[]{typeof(string)});

            Assert.IsNotNull(realType);

            object t = realType.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            realType.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, t, 
                new object[] {"xxx"} );

            Assert.AreEqual("xxx", realType.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
        }

        [TestMethod]
        public void Builder_Delegate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddDelegates(
                    Define.Delegate(typeof(int), MemberAttributes.Public, (string s)=>"xxx"),
                    Define.Delegate(MemberAttributes.Public, (string s)=>"xxx1"),
                    Define.Delegate(typeof(string), MemberAttributes.Public, () => "xxx2")
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
        public void Builder_Event()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddDelegate(typeof(int), MemberAttributes.Public, (string s) => "xxxDelegate")
                .AddEvents(
                    Define.Event("xxxDelegate", MemberAttributes.Public, "Event1")
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
        public void Builder_Ctors()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddFields(
                    Define.Field(MemberAttributes.Private, typeof(string), "_s"),
                    Define.Field(MemberAttributes.Private, typeof(int), "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Emit.assignField("_s", () => "xxx"),
                        Emit.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (string s) => s),
                        Emit.assignField("_i", (int i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, "TestClass", () => "Create", 
                    Emit.@return(() => CodeDom.@new("TestClass", 100, "yyy")))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual("xxx", TestClass.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
            Assert.AreEqual(10, TestClass.InvokeMember("I", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));

            t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null,
                new object[] { 100, "yyy" });

            Assert.IsNotNull(t);

            Assert.AreEqual("yyy", TestClass.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
            Assert.AreEqual(100, TestClass.InvokeMember("I", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));

            t = TestClass.InvokeMember("Create", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod, null, null, null);

            Assert.IsNotNull(t);

            Assert.AreEqual("yyy", TestClass.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
            Assert.AreEqual(100, TestClass.InvokeMember("I", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
        }

        [TestMethod]
        public void Builder_Attribute()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddAttribute(Define.Attribute(typeof(SerializableAttribute)))
                .AddFields(
                    Define.Field(MemberAttributes.Private, typeof(string), "_s"),
                    Define.Field(MemberAttributes.Private, typeof(int), "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Emit.assignField("_s", () => "xxx"),
                        Emit.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (string s) => s),
                        Emit.assignField("_i", (int i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(object), (MemoryStream ms) => "Deserialize", 
                    Emit.declare("f", () => new BinaryFormatter()), 
                    Emit.stmt((MemoryStream ms) => ms.Seek(0, SeekOrigin.Begin)), 
                    Emit.@return((BinaryFormatter f, MemoryStream ms) => f.Deserialize(ms))
                )
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);

            Assert.IsNotNull(t);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();

                f.Serialize(ms, t);

                //object t2 = TestClass.GetMethod("Deserialize").Invoke(null, new object[] { ms });

                //Assert.IsNotNull(t2);

                //Assert.AreEqual(
                //    TestClass.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null),
                //    TestClass.InvokeMember("S", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t2, null)
                //);

                //Assert.AreEqual(
                //    TestClass.InvokeMember("I", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null),
                //    TestClass.InvokeMember("I", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t2, null)
                //);
            }
        }
        
        [TestMethod]
        public void Builder_Interface()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
            .AddClass(Define.Class("TestClass", TypeAttributes.Public)
                .Implements(typeof(IDisposable))
                .AddMethod(MemberAttributes.Private, ()=>"Dispose", 
                    Emit.@throw(()=>new NotImplementedException())
                ).Implements(typeof(IDisposable))
            ).AddInterface(Define.Interface("Ixxx")
                .AddMethod(MemberAttributes.Public, ()=>"First")
                .AddMethod(MemberAttributes.Public, typeof(DateTime), () => "Second")
                .AddProperty("TestClass", MemberAttributes.Public, "Third")
                .AddGetProperty("TestClass", MemberAttributes.Public, "Fifth")
                .AddEvent(typeof(EventHandler), MemberAttributes.Public, "Fourth")
            ).AddClass("xxx").Implements(new CodeTypeReference("Ixxx"))
                .AddMethod(MemberAttributes.Private, ()=>"First",
                    Emit.@throw(()=>new NotImplementedException())
                ).Implements(new CodeTypeReference("Ixxx"))
                .AddMethod(MemberAttributes.Public, typeof(DateTime), () => "Second", 
                    Emit.@throw(() => new NotImplementedException())
                ).Implements(new CodeTypeReference("Ixxx"))
                .AddProperty("TestClass", MemberAttributes.Public, "Third",
                    CodeDom.CombineStmts(Emit.@throw(() => new NotImplementedException())),
                    Emit.@throw(() => new NotImplementedException())
                ).Implements("Ixxx")
                .AddGetProperty("TestClass", MemberAttributes.Public, "Fifth",
                    Emit.@throw(() => new NotImplementedException())
                ).Implements(new CodeTypeReference("Ixxx"))
                .AddEvent(typeof(EventHandler), MemberAttributes.Public, "Fourth"
                ).Implements("Ixxx")
                .AddField(typeof(int), "_z", () => 100)
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

            ass = c.Compile(CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            TestClass = ass.GetType("Samples.xxx");

            Assert.IsNotNull(TestClass);

        }

        [TestMethod]
        public void Builder_Struct()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddStruct(Define.Struct("xxx")
                .AddField(typeof(bool), "_x")
                .AddField(typeof(int), "_y")
                .AddCtor((bool x) => MemberAttributes.Public,
                    Emit.assignField("_x", (bool x) => x),
                    Emit.assignField("_y", ()=> 100)
                )
                .AddGetProperty(typeof(int), MemberAttributes.Public, "Z", "_y")
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.xxx");

            Assert.IsNotNull(TestClass);

            object t = TestClass.InvokeMember(null, BindingFlags.CreateInstance, null, null,
                new object[] { false });

            Assert.IsNotNull(t);

            Assert.AreEqual(100, TestClass.InvokeMember("Z", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t, null));
        }

        [TestMethod]
        public void Builder_Enum()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddEnum(Define.Enum("ee")
                .AddFields(
                    Define.StructField("Xx"),
                    Define.StructField("Yy"),
                    Define.StructField("Zz", ()=>100)
                )
            ).AddEnum("rr").AddAttribute(typeof(FlagsAttribute))
                .AddFields(
                    Define.StructField("Xx"),
                    Define.StructField("Yy"),
                    Define.StructField("Zz")
            ).AddClass("cls").AddMethod(MemberAttributes.Static | MemberAttributes.Public, ()=>"foo",
                Emit.declare("rr", "e"),
                Emit.assignVar("e", () => CodeDom.Field(new CodeTypeReference("rr"), "Xx"))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);

            Type rrClass = ass.GetType("Samples.rr");

            Assert.IsNotNull(rrClass);
        }

        [TestMethod]
        public void Builder_Operator()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("ee")
                .AddOperators(
                    Define.Operator(new CodeTypeReference(typeof(int)),
                        (DynType t) => CodeDom.TypedSeq(OperatorType.Implicit,t.SetType("ee")),
                        Emit.@return(()=>10)
                    )
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }

        [TestMethod]
        public void TestPartialMethodsCS()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(new CodePartialMethod(Define.Method(MemberAttributes.Public,
                    ()=>"foo"
                )))
                .AddMember(Define.PartialMethod(MemberAttributes.Public,
                    () => "foo2"
                ))
                )
                .AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(new CodePartialMethod(Define.Method(MemberAttributes.Public,
                    () => "foo", Emit.stmt(()=>Console.WriteLine())
                )))
                .AddMember(Define.PartialMethod(MemberAttributes.Public,
                    () => "foo2", Emit.stmt(() => Console.WriteLine())
                ))

                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }

        [TestMethod]
        public void TestPartialMethodsVB()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(new CodePartialMethod(Define.Method(MemberAttributes.Public,
                    () => "foo"
                )))
                //.AddMember(new CodePartialMethod(Define.Method(MemberAttributes.Public, typeof(string),
                //    () => "foo2"
                //)))
                )
                .AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(new CodePartialMethod(Define.Method(MemberAttributes.Public,
                    () => "foo", Emit.stmt(() => Console.WriteLine())
                )))
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile(null, CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }

        [TestMethod]
        public void TestDefaultProperty()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(Define.GetProperty(new CodeTypeReference(typeof(string)), MemberAttributes.Family,
                    (int idx)=>"prop", true, 
                        Emit.@return((int idx)=>idx.ToString())
                ))
                .AddMember(Define.Property(new CodeTypeReference(typeof(string)), MemberAttributes.Family,
                    (int idx) => "prop2", false,
                        CodeDom.CombineStmts(Emit.@return((int idx) => idx.ToString())),
                        Emit.declare("i", (int idx)=>idx)
                ))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            var ass = c.Compile(null, CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }

        [TestMethod]
        public void TestCustomEvent()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("ee", TypeAttributes.Public, true)
                .AddMember(Define.Event(new CodeTypeReference(typeof(EventHandler)), MemberAttributes.Family,
                    "Event1", Define.GetProperty("", default(MemberAttributes), "",
                        Emit.stmt(()=>Console.WriteLine("add"))
                    ),
                    Define.GetProperty("", default(MemberAttributes), "",
                        Emit.stmt(() => Console.WriteLine("remove"))
                    ),
                    Define.Method(default(MemberAttributes), (object sender, EventArgs args)=>"",
                        Emit.stmt(() => Console.WriteLine("raise"))
                    )
                ))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            var ass = c.Compile(null, CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);

            ass = c.Compile(null, CodeDomGenerator.Language.CSharp);

            Assert.IsNotNull(ass);

            eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }

        [TestMethod]
        public void TestReadonlyField()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("ee", TypeAttributes.Public)
                .AddMember(Define.ReadOnlyField(MemberAttributes.Family, new CodeTypeReference(typeof(int)), "fld1"))
                .AddMember(Define.ReadOnlyField(MemberAttributes.Private | MemberAttributes.Static, new CodeTypeReference(typeof(int)), "fld2"))
                .AddMember(Define.Const(MemberAttributes.Private, "cns1", ()=>1))
                .AddMember(Define.Const(MemberAttributes.Public, "cns2", () => "hi!"))
            );

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            var ass = c.Compile(null, CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            Type eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);

            ass = c.Compile(null, CodeDomGenerator.Language.CSharp);

            Assert.IsNotNull(ass);

            eeClass = ass.GetType("Samples.ee");

            Assert.IsNotNull(eeClass);
        }
    }
}
