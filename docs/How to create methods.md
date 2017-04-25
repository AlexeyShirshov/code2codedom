Method can be defined via Define static class.
{code:c#}
CodeMemberMethod method = Define.Method(MemberAttributes.Public, () => "foo");
{code:c#}
There is also extension AddMethod in CodeTypeDeclaration class
{code:c#}
CodeTypeDeclaration cls = Define.Class("cls");
CodeMemberMethod method = cls.AddMethod(Define.Method(MemberAttributes.Public, () => "foo"));
{code:c#}
AddMethod has overload to avoid Define.Method call
{code:c#}
CodeTypeDeclaration cls = Define.Class("cls");
CodeMemberMethod method = cls.AddMethod(MemberAttributes.Public, () => "foo");
{code:c#}
So the whole program my looks like
{code:c#}
static void Main()
{
    var c = new CodeDomGenerator();

    c.AddNamespace("TestNS").AddClass("cls")
        .AddMethod(MemberAttributes.Public, () => "foo");

    Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));
}
{code:c#}