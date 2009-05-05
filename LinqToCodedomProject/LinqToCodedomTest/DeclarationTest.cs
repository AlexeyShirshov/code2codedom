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
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(
              c.Class("TestClass")
                .AddMethod(typeof(int), MemberAttributes.Public, (int a) => "Test",
                    Builder.@return((ParamRef<int> a) => a + 100)
                )
                .AddMethod(
                    Define.Method(typeof(int), MemberAttributes.Public, (int a) => "Test1",
                        Builder.@return(() =>
                            Builder.VarRef<int>("a") +
                            Builder.@this.Call<int>("Test")(3))
                    )
                )
            ).AddClass(c.Class("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Builder.declare("TestClass", "cc"),
                    Builder.stmt(() => Builder.Call(Builder.VarRef("cc"), "Test1")(3))
                )
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo2",
                    Builder.declare("TestClass", "cc", () => Builder.@new("TestClass")),
                    Builder.stmt((Var cc) => cc.Call("Test1")(3))
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

            Assert.AreEqual(104, TestClass.InvokeMember("Test", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }));

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 1 }));
        }

        [TestMethod]
        public void Builder_GetProperty()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(
              c.Class("TestClass")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "Test",
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

            Assert.AreEqual(104, TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }));

        }

        [TestMethod]
        public void Builder_Property()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(
              c.Class("TestClass")
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s")
                )
                .AddProperty(
                    Define.Property(typeof(string), MemberAttributes.Public, "Test",
                       Builder.CombineStmts(Builder.@return(() => Builder.@this.Field<string>("_s"))),
                       Builder.assignField("_s", (ParamRef<string> value) => value)
                    )
                )
                .AddProperty(
                    Define.Property(typeof(string), MemberAttributes.Public, "Test2", "_s")
                )
                .AddProperty(typeof(string), MemberAttributes.Public, "Test3", "_s")
                .AddProperty("TestClass", MemberAttributes.Public, "Test4",
                   Builder.CombineStmts(Builder.@return(() => Builder.@this)),
                   Builder.@throw(() => new NotImplementedException(
                       Builder.Property<string>(Builder.VarRef("value"), "Test")
                   ))
                )
                .AddMethod(typeof(string), MemberAttributes.Public, (int a) => "Test1",
                    Builder.assignProperty("Test", () => Guid.NewGuid().ToString()),
                    Builder.@return((ParamRef<int> a) =>
                        a.ToString() + Builder.@this.Property<string>("Test"))
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

            string s = TestClass.InvokeMember("Test1", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, t,
                new object[] { 4 }) as string;

            Assert.IsNotNull(s);

            string TestProperty = s.Substring(1);

            Assert.AreEqual(TestProperty, TestClass.InvokeMember("Test", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty, null, t, null));
        }

        [TestMethod]
        public void Builder_GenericMethod()
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
                    Builder.@return((Var cc)=> cc)
                ).Generic("T")
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

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
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .Generic("T")
                .AddFields(
                    Define.Field("T", MemberAttributes.Private, "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

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
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .AddDelegates(
                    Define.Delegate(typeof(int), MemberAttributes.Public, (string s)=>"xxx"),
                    Define.Delegate(MemberAttributes.Public, (string s)=>"xxx1"),
                    Define.Delegate(typeof(string), MemberAttributes.Public, () => "xxx2")
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
        public void Builder_Event()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .AddDelegate(typeof(int), MemberAttributes.Public, (string s) => "xxxDelegate")
                .AddEvents(
                    Define.Event("xxxDelegate", MemberAttributes.Public, "Event1")
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
        public void Builder_Ctors()
        {
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Builder.assignField("_s", () => "xxx"),
                        Builder.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Builder.assignField("_s", (VarRef<string> s) => s),
                        Builder.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod("TestClass", MemberAttributes.Static | MemberAttributes.Public, ()=>"Create",
                    Builder.@return(()=>Builder.@new("TestClass", 100, "yyy"))
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
            var c = new CodeDom();

            c.AddNamespace("Samples").AddClass(c.Class("TestClass")
                .AddAttribute(Define.Attribute(typeof(SerializableAttribute)))
                .AddFields(
                    Define.Field(typeof(string), MemberAttributes.Private, "_s"),
                    Define.Field(typeof(int), MemberAttributes.Private, "_i")
                )
                .AddCtor(
                    Define.Ctor(() => MemberAttributes.Public,
                        Builder.assignField("_s", () => "xxx"),
                        Builder.assignField("_i", () => 10)
                    )
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Builder.assignField("_s", (VarRef<string> s) => s),
                        Builder.assignField("_i", (VarRef<int> i) => i)
                    )
                )
                .AddGetProperty(typeof(string), MemberAttributes.Public, "S", "_s").Comment("This is a comment")
                .AddGetProperty(typeof(int), MemberAttributes.Public, "I", "_i").Document("This is a documentation")
                .AddMethod(typeof(object), MemberAttributes.Public | MemberAttributes.Static, (MemoryStream ms)=>"Deserialize", 
                    Builder.declare("f",()=>new BinaryFormatter()),
                    Builder.stmt((VarRef<MemoryStream> ms)=>ms.v.Seek(0, SeekOrigin.Begin)),
                    Builder.@return((VarRef<BinaryFormatter> f, VarRef<MemoryStream> ms)=>f.v.Deserialize(ms))
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
            var c = new CodeDom();

            c.AddNamespace("Samples")
            .AddClass(c.Class("TestClass", MemberAttributes.Public)
                .Implements(typeof(IDisposable))
                .AddMethod(MemberAttributes.Private, ()=>"Dispose", 
                    Builder.@throw(()=>new NotImplementedException())
                ).Implements(typeof(IDisposable))
            ).AddInterface(c.Interface("Ixxx")
                .AddMethod(MemberAttributes.Public, ()=>"First")
                .AddMethod(typeof(DateTime), MemberAttributes.Public, () => "Second")
                .AddProperty("TestClass", MemberAttributes.Public, "Third")
                .AddGetProperty("TestClass", MemberAttributes.Public, "Fifth")
                .AddEvent(typeof(EventHandler), MemberAttributes.Public, "Fourth")
            );

            Console.WriteLine(c.GenerateCode(CodeDom.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDom.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.TestClass");

            Assert.IsNotNull(TestClass);

        }

        [TestMethod]
        public void Builder_Struct()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Builder_Enum()
        {
            var c = new CodeDom();

            Assert.Inconclusive();
        }
    }
}
