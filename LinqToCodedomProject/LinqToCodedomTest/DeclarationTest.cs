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
                .AddMethod(typeof(int), MemberAttributes.Public, (int a) => "Test",
                    Emit.@return((ParamRef<int> a) => a + 100)
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public, (int a) => "Test1",
                        Emit.@return(() =>
                            CodeDom.VarRef<int>("a") +
                            CodeDom.@this.Call<int>("Test")(3))
                    )
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
                .AddMethod(typeof(int), MemberAttributes.Public, (int a) => "Test1",
                    Emit.@return((ParamRef<int> a) =>
                        a + CodeDom.@this.Property<int>("Test"))
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

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }));

        }

        [TestMethod]
        public void Builder_Property()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s")
                )
                .AddProperty(
                    Define.Property(typeof(string), MemberAttributes.Public, "Test",
                       CodeDom.CombineStmts(Emit.@return(() => CodeDom.@this.Field<string>("_s"))),
                       Emit.assignField("_s", (ParamRef<string> value) => value)
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
                .AddMethod(typeof(string), MemberAttributes.Public, (int a) => "Test1",
                    Emit.assignProperty("Test", () => Guid.NewGuid().ToString()),
                    Emit.@return((ParamRef<int> a) =>
                        a.ToString() + CodeDom.@this.Property<string>("Test"))
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
                .AddMethod(typeof(int), MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("cls1", "cc", () => CodeDom.@new("cls1")),
                    Emit.@return((Var cc) => cc.Property<int>("I"))
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls2");

            Assert.IsNotNull(TestClass);

            int s = (int)TestClass.InvokeMember("foo", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.InvokeMethod, 
                null, null, null);

            Assert.AreEqual(10, s);
        }

        [TestMethod]
        public void Builder_GenericMethod()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass").Generic("T")
                .AddFields(
                    Define.Field("T", MemberAttributes.Private, "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            ).AddClass(Define.Class("cls")
                .AddMethod(CodeDom.TypeRef("TestClass", "T"), MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare(CodeDom.TypeRef("TestClass", "T"), "cc",
                        () => CodeDom.@new(CodeDom.TypeRef("TestClass", "T"))),
                    Emit.@return((Var cc)=> cc)
                ).Generic("T")
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
                    Define.Field("T", MemberAttributes.Private, "_s")
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
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Emit.assignField("_s", () => "xxx"),
                        Emit.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (VarRef<string> s) => s),
                        Emit.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod("TestClass", MemberAttributes.Static | MemberAttributes.Public, ()=>"Create",
                    Emit.@return(() => CodeDom.@new("TestClass", 100, "yyy"))
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
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Emit.assignField("_s", () => "xxx"),
                        Emit.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (VarRef<string> s) => s),
                        Emit.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod(typeof(object), MemberAttributes.Public | MemberAttributes.Static, (MemoryStream ms)=>"Deserialize", 
                    Emit.declare("f",()=>new BinaryFormatter()),
                    Emit.stmt((VarRef<MemoryStream> ms)=>ms.v.Seek(0, SeekOrigin.Begin)),
                    Emit.@return((VarRef<BinaryFormatter> f, VarRef<MemoryStream> ms)=>f.v.Deserialize(ms))
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
            .AddClass(Define.Class("TestClass", MemberAttributes.Public)
                .Implements(typeof(IDisposable))
                .AddMethod(MemberAttributes.Private, ()=>"Dispose", 
                    Emit.@throw(()=>new NotImplementedException())
                ).Implements(typeof(IDisposable))
            ).AddInterface(Define.Interface("Ixxx")
                .AddMethod(MemberAttributes.Public, ()=>"First")
                .AddMethod(typeof(DateTime), MemberAttributes.Public, () => "Second")
                .AddProperty("TestClass", MemberAttributes.Public, "Third")
                .AddGetProperty("TestClass", MemberAttributes.Public, "Fifth")
                .AddEvent(typeof(EventHandler), MemberAttributes.Public, "Fourth")
            ).AddClass("xxx").Implements("Ixxx")
                .AddMethod(MemberAttributes.Public, ()=>"First",
                    Emit.@throw(()=>new NotImplementedException())
                ).Implements("Ixxx")
                .AddMethod(typeof(DateTime), MemberAttributes.Public, () => "Second",
                    Emit.@throw(() => new NotImplementedException())
                ).Implements("Ixxx")
                .AddProperty("TestClass", MemberAttributes.Public, "Third",
                    CodeDom.CombineStmts(Emit.@throw(() => new NotImplementedException())),
                    Emit.@throw(() => new NotImplementedException())
                ).Implements("Ixxx")
                .AddGetProperty("TestClass", MemberAttributes.Public, "Fifth",
                    Emit.@throw(() => new NotImplementedException())
                ).Implements("Ixxx")
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

        }

        [TestMethod]
        public void Builder_Struct()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddStruct(Define.Struct("xxx")
                .AddField(typeof(bool), "_x")
                .AddField(typeof(int), "_y")
                .AddCtor((bool x) => MemberAttributes.Public,
                    Emit.assignField("_x", (VarRef<bool> x) => x),
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

            object t = TestClass.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null,
                new object[] { false });

            Assert.IsNotNull(t);

            Assert.AreEqual(100, TestClass.InvokeMember("Z", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
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
    }
}
