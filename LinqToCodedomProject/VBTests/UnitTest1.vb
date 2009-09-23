Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LinqToCodedom
Imports LinqToCodedom.Extensions
Imports LinqToCodedom.Generator
Imports System.CodeDom

<TestClass()> Public Class UnitTest1

    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' Use ClassCleanup to run code after all tests in a class have run
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' Use TestInitialize to run code before running each test
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Use TestCleanup to run code after each test has run
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

    <TestMethod()> Public Sub TestIsOperator()
        Dim c = New CodeDomGenerator()

        c.AddNamespace("Samples").AddClass("cls") _
                .AddMethod(MemberAttributes.Public Or MemberAttributes.Static, Function() "foo", _
                    Emit.declare("d1", Function() CodeDom.Is(String.Empty, GetType(String))), _
                    Emit.declare("d2", Function() CodeDom.IsNot(String.Empty, GetType(String))) _
                )

        Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.CSharp))

        Console.WriteLine(c.GenerateCode(CodeDomGenerator.Language.VB))

        Dim ass = c.Compile()

        Assert.IsNotNull(ass)

        Dim TestClass As Type = ass.GetType("Samples.cls")

        Assert.IsNotNull(TestClass)

    End Sub

End Class
