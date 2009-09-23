using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToCodedom;
using LinqToCodedom.Generator;
using System.CodeDom;
using LinqToCodedom.Extensions;
using System.Reflection;
using System.Linq.Expressions;

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
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare(typeof(int[]), "d"),
                    Emit.declare("d2", () => new int[] { 1, 2, 3 }),
                    Emit.assignVar("d", () => new int[] { 3, 4 }),
                    Emit.declare("d3", (int[] d) => d[0]),
                    Emit.declare("d4", ()=>new int[10])
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
        public void ArrayComplexTypeCreate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("cls[]", "d"),
                    Emit.declare(CodeDom.TypeRef(typeof(List<>), "cls"), "d2"),
                    Emit.assignVar("d", (Var d2) => d2.Call("ToArray"))
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
        public void ArrayIndexer()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("d", () => new int[] { 1, 2, 3 }),
                    Emit.declare("d2", (int[] d) => d[0])
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
        public void ArrayComplexTypeIndexer()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("cls[]", "d"),
                    Emit.declare("cls", "d2"),
                    Emit.assignVar("d2", (Var d) => d.ArrayGet(1))
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
        public void ArrayComplexTypeMultiIndexer()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("cls[,]", "d"),
                    Emit.declare("cls", "d2"),
                    Emit.assignVar("d2", (Var d) => d.ArrayGet(1,0))
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
        public void ArrayComplexTypeJaggedIndexer()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("cls[][]", "d"),
                    Emit.declare("cls", "d2"),
                    Emit.assignVar("d2", (Var d) => d.JaggedArrayGet(1, 0))
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
        public void MultidimensionalArray()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("d", () => new int[2, 2]),
                    Emit.declare("d2", (int[,] d) => d[0, 1])
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
        public void JaggedArray()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.declare("d", () => new int[][] { new int[] { 1 }, new int[] { 2 } }),
                    Emit.declare("d2", (int[][] d) => d[0][0])
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
        public void ObjectCreate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass(Define.Class("TestClass")
                .AddFields(
                    Define.Field(MemberAttributes.Private, typeof(string), "_s"),
                    Define.Field(MemberAttributes.Private, typeof(int), "_i")
                )
                .AddCtor(
                    Define.Ctor((int i, string s) => MemberAttributes.Public,
                        Emit.assignField("_s", (string s) => s),
                        Emit.assignField("_i", (int i) => i)
                    )
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, "TestClass", () => "Create", Emit.@return(() => CodeDom.@new("TestClass", 100, "yyy")))
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
                    Define.Field(MemberAttributes.Private, "T", "_s")
                )
                .AddProperty("T", MemberAttributes.Public, "S", "_s")
            ).AddClass(Define.Class("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, CodeDom.TypeRef("TestClass", "T"), () => "foo", 
                    Emit.declare(CodeDom.TypeRef("TestClass", "T"), "cc",
                        () => CodeDom.@new(CodeDom.TypeRef("TestClass", "T"))), 
                    Emit.@return((Var cc) => cc))
                .Generic("T")
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
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Public, () => "foo",
                    Emit.declare(typeof(object), "d"),
                    Emit.assignVar("d", () => 10d),
                    Emit.declare("dr", (object d) => (decimal)d),
                    Emit.declare("dr2", (object d) => CodeDom.cast(typeof(decimal), d)),
                    Emit.stmt((object d) => CodeDom.cast<Var>("cls", d).Call("foo"))
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
        public void TypeOfExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, typeof(string), (object o) => "foo", 
                    Emit.ifelse((object o) => o.GetType() == typeof(int),
                        CodeDom.CombineStmts(Emit.@return(() => "int")),
                        Emit.@return(() => "other")))
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);

        }

        [TestMethod]
        public void DefaultExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "foo",
                    Emit.stmt(() => CodeDom.Call(CodeDom.TypeRef("cls"), "zoo")(default(int)))
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Private, (int i) => "zoo",
                    Emit.stmt((int i) => Console.WriteLine(i))
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
        public void StaticDelegate()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public,
                    () => "foo",
                    Emit.declare(typeof(EventHandler), "h"),
                    Emit.assignDelegate("h", "zoo"),
                    Emit.stmt((Var h) => h.Call()(null, null))
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Private, (object sender, EventArgs args) => "zoo")
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
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, typeof(string), () => "foo", Emit.declare("h2", () => new Func<string>("aaa".ToString)), Emit.@return((Func<string> h2) => h2()))
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
                .AddMethod(MemberAttributes.Public, typeof(string), () => "foo", Emit.declare(typeof(Func<int, string>), "h2"), Emit.assignDelegate("h2", CodeDom.@this, "zoo"), Emit.@return((Func<int, string> h2) => h2(10)))
                .AddMethod(MemberAttributes.Public, typeof(string), (int i) => "zoo", Emit.@return((int i) => i.ToString()))
            .AddClass("cls2")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), (DynType cc) => "foo" + cc.SetType("cls"), Emit.declare(typeof(Func<int, string>), "h2"), Emit.assignDelegate("h2", CodeDom.VarRef("cc"), "zoo"), Emit.@return((Func<int, string> h2) => h2(100)))
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), (DynType cc, DynType c2) => "foo" + cc.SetType("cls") + c2.SetType(typeof(string)), Emit.declare(typeof(Func<int, string>), "h2"), Emit.assignDelegate("h2", CodeDom.VarRef("cc"), "zoo"), Emit.@return((Func<int, string> h2, string c2) => h2(100) + c2))
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
                null, t2, new object[] { t });

            Assert.AreEqual("100", s2);
        }

        [TestMethod]
        public void WorkingWithEvent()
        {
            var c = new CodeDomGenerator();

            c.AddReference("System.Core.dll").AddNamespace("Samples").AddClass("cls")
                .AddEvent(typeof(Action), MemberAttributes.Public, "ev")
                .AddMethod(MemberAttributes.Public, () => "raise",
                    Emit.declare("cls2", "cc", () => CodeDom.@new("cls2")),
                    Emit.attachDelegate(CodeDom.@this, "ev", CodeDom.VarRef("cc"), "zoo"),
                    Emit.attachDelegate(CodeDom.@this, "ev", "cls2.foo"),
                    Emit.stmt(() => CodeDom.@this.Raise("ev")()),
                    Emit.detachDelegate(CodeDom.@this, "ev", CodeDom.VarRef("cc"), "zoo")
                )
            .AddClass("cls2")
                .AddMethod(MemberAttributes.Public, () => "zoo",
                    Emit.stmt(() => Console.WriteLine("ok"))
                )
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
                    Emit.stmt(() => Console.WriteLine("ok"))
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type cls = ass.GetType("Samples.cls");

            Assert.IsNotNull(cls);
        }

        [TestMethod]
        public void ParamByRef()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public,
                    (RefParam<int> i, DynTypeRef j) => "foo" + j.SetType(typeof(string)),
                    Emit.assignVar("i", () => 10),
                    Emit.assignVar("j", () => "zzz")
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type cls = ass.GetType("Samples.cls");

            Assert.IsNotNull(cls);

            object[] args = new object[] { 0, "" };
            System.Reflection.ParameterModifier p = new System.Reflection.ParameterModifier(2);
            p[0] = true;
            p[1] = true;

            string s = (string)cls.InvokeMember("foo",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static,
                null, new System.Reflection.ParameterModifier[] { p }, args);

            Assert.AreEqual(10, args[0]);
            Assert.AreEqual("zzz", args[1]);

        }

        [TestMethod]
        public void ParamArrayTest()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public,
                    (ParamArray<int> i) => "foo",
                    Emit.declare("j", () => "zzz")
                )
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public,
                    (ParamArray<int[]> i) => "foo2",
                    Emit.declare("js", () => "zzz")
                )
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type cls = ass.GetType("Samples.cls");

            Assert.IsNotNull(cls);
        }

        [TestMethod]
        public void AsExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "foo", 
                    Emit.declare("d", () => string.Empty as IComparable)
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
        public void IsExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, () => "foo",
                    Emit.declare("d", () => string.Empty is IComparable)
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
        public void XorExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples").AddClass("cls")
                .AddMethod(MemberAttributes.Static | MemberAttributes.Public, (int i) => "foo",
                    Emit.declare("d", (int i) => i ^ 5)
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
        public void ThisTest()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
                .AddClass("cls").Implements(typeof(IServiceProvider))
                .AddMethod(MemberAttributes.Public, typeof(object), (Type t) => "GetService",
                    Emit.@return(() => CodeDom.@this.cast<IServiceProvider>())
                ).Implements(typeof(IServiceProvider))
            ;

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);

        }

        [TestMethod]
        public void TestGetExpression()
        {
            CodeExpression exp = CodeDom.GetExpression(()=>1);

            Assert.IsTrue(exp is CodePrimitiveExpression);
            Assert.AreEqual(1, ((CodePrimitiveExpression)exp).Value);

            CodeExpression exp2 = CodeDom.GetExpression((int g) => 1+g);

            Assert.IsTrue(exp2 is CodeBinaryOperatorExpression);

            CodeExpression exp3 = CodeDom.GetExpression((int g) => CodeDom.@this.Field<int>("d")+g-10);

            Assert.IsTrue(exp3 is CodeBinaryOperatorExpression);

            CodeExpression exp4 = CodeDom.GetExpression(() => CodeDom.Call(exp, "ToString")());

            Assert.IsTrue(exp4 is CodeMethodInvokeExpression);
            Assert.AreEqual(exp, ((CodeMethodInvokeExpression)exp4).Method.TargetObject);

            CodeExpression exp5 = CodeDom.GetExpression(() => CodeDom.Call(exp3, "ToString")());

            Assert.IsTrue(exp5 is CodeMethodInvokeExpression);
            Assert.AreEqual(exp3, ((CodeMethodInvokeExpression)exp5).Method.TargetObject);
        }

        [TestMethod]
        public void TestGetExpressionInject()
        {
            CodeExpression exp = CodeDom.GetExpression(() => 1);

            Assert.IsTrue(exp is CodePrimitiveExpression);
            Assert.AreEqual(1, ((CodePrimitiveExpression) exp).Value);

            CodeExpression exp2 = CodeDom.GetExpression((int g) => CodeDom.InjectExp<int>(0) + g, exp);

            Assert.IsTrue(exp2 is CodeBinaryOperatorExpression);
            Assert.AreSame(exp, ((CodeBinaryOperatorExpression)exp2).Left);
            Assert.IsTrue(((CodeBinaryOperatorExpression)exp2).Right is CodeVariableReferenceExpression);
        }

        [TestMethod]
        public void TestNotExpression()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
                .AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (bool f) => "foo",
                    Emit.declare("b", (bool f) => !f),
                    Emit.declare("s", (bool f) => !(f && false))
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
        public void TestTernary()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
                .AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (bool f) => "foo",
                    Emit.declare("b", (bool f) => !f),
                    Emit.assignVar("b", (bool f)=>f?true:false)
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
        public void TestLambda()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
                .AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (bool f) => "foo",
                    Emit.declare("a", () => CodeDom.Lambda(()=>1)),
                    Emit.declare("b", () => CodeDom.Lambda(() => Console.WriteLine("hi!"))),
                    Emit.declare("c", () => CodeDom.Lambda((int aa) => Console.WriteLine(aa), 
                        new LambdaParam(typeof(int), "aa"))),
                    Emit.stmt((int a)=>CodeDom.CallDelegate(CodeDom.VarRef("c").Call("Compile"))(
                        CodeDom.CallDelegate(CodeDom.VarRef("a").Call("Compile"))
                        ))
                )
            ;

            
            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp));

            c.AddReference("System.Core.dll");

            var ass = c.Compile();

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }

        [TestMethod]
        public void TestLambdaVB()
        {
            var c = new CodeDomGenerator();

            c.AddNamespace("Samples")
                .AddClass("cls")
                .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (bool f) => "foo",
                    Emit.declare("a", () => CodeDom.Lambda(() => 1)),
                    Emit.declare("c", () => CodeDom.Lambda((int aa) => aa+10,
                        new LambdaParam(typeof(int), "aa"))),
                    Emit.stmt((int a) => CodeDom.CallDelegate(CodeDom.VarRef("c").Call("Compile"))(
                        CodeDom.CallDelegate(CodeDom.VarRef("a").Call("Compile"))
                        ))
                )
            ;


            c.AddReference("System.Core.dll");

            Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB));

            var ass = c.Compile(CodeDomGenerator.Language.VB);

            Assert.IsNotNull(ass);

            Type TestClass = ass.GetType("Samples.cls");

            Assert.IsNotNull(TestClass);
        }
    }
}
