For type and member declaration there is Define static class.
Below is example how to define class cls
{code:c#}
CodeTypeDeclaration cls = Define.Class("cls");
{code:c#}
There is another approach to define class - use extension method AddClass of type CodeNamespace
{code:c#}
CodeNamespace ns = new CodeNamespace("TestNS");
CodeTypeDeclaration cls = ns.AddClass(Define.Class("cls"));
{code:c#}
AddClass extension also can accept class name parameter
{code:c#}
CodeNamespace ns = new CodeNamespace("TestNS");
CodeTypeDeclaration cls = ns.AddClass("cls");
{code:c#}
The code can be generated via CodeDomGenerator class. CodeDomGenerator has AddNamespace method so the whole program may looks like
{code:c#}
static void Main()
{
    var c = new CodeDomGenerator();

    c.AddNamespace("TestNS").AddClass("cls");

    Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));
}
{code:c#}
And the resulting code is
{code:c#}
namespace TestNS {
    public class cls {
    }
}
{code:c#}
All high level abstructions and their relationships defined in my [blog](http://wise-orm.com/category/expression2codedom.aspx).