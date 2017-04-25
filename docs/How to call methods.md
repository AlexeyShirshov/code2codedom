To call static method of the current class you can use Call method in CodeDom helper class. More about CodeDom, Emit, Define classes see in my [blog](http://wise-orm.com/post/Expression-to-CodeDOM.aspx).
In example below we define two methods. Method foo calls method zoo.
{code:c#}
static void Main()
{
    var c = new CodeDomGenerator();

    c.AddNamespace("TestNS").AddClass("cls")
        .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "foo",
            Emit.stmt(()=>CodeDom.Call("zoo")())
        )
        .AddMethod(MemberAttributes.Public | MemberAttributes.Static, () => "zoo")
    ;

    Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));
}
{code:c#}
Instance methods can be invoked via @this special property in CodeDom class. 
{code:c#}
.AddMethod(MemberAttributes.Public, () => "foo",
    Emit.stmt(()=>CodeDom.@this.Call("zoo")())
)
.AddMethod(MemberAttributes.Public, () => "zoo")
}
{code:c#}
The resulting c# code
{code:c#}
namespace TestNS {
    public class cls {

        public virtual void foo() {
            this.zoo();
        }

        public virtual void zoo() {
        }
    }
}
{code:c#}
And here is VB.NET code
{code:vb.net}
Namespace TestNS

    Public Class cls

        Public Overridable Sub foo()
            Me.zoo
        End Sub

        Public Overridable Sub zoo()
        End Sub
    End Class
End Namespace
{code:vb.net}