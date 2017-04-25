At this point (if you explore howtos in series) we know how to declare parameters and variables, assign values to them, call methods. It is enough to create implementation of Fibonacci algorithm. Here it is
{code:c#}
static void Main()
{
    var c = new CodeDomGenerator();

    c.AddNamespace("TestNS").AddClass("Fibonacci")
        .AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int x) => "Calc",
            Emit.@if((int x) => x <= 1, 
                Emit.@return(()=>1)),
            Emit.@return((int x) => 
                CodeDom.Call<int>("Calc")(x - 1) + CodeDom.Call<int>("Calc")(x - 2))
            )
    ;

    Console.WriteLine(c.GenerateCode(LinqToCodedom.CodeDomGenerator.Language.CSharp));
}
{code:c#}
The program generates
{code:c#}
namespace TestNS {

    public class Fibonacci {

        public static void Calc(int x) {
            if ((x <= 1)) {
                return 1;
            }
            return (Calc((x - 1)) + Calc((x - 2)));
        }
    }
}
{code:c#}
For those of you who have a luck don't work with CodeDOM here is the same task in pure CodeDOM.
{code:c#}
static void Main()
{
    CodeMemberMethod Calc = new CodeMemberMethod()
    {
        Name = "Calc",
        Attributes = MemberAttributes.Public | MemberAttributes.Static
    };

    Calc.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "x"));

    Calc.Statements.Add(
        new CodeConditionStatement(
            new CodeBinaryOperatorExpression(
                new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.LessThanOrEqual,
                new CodeSnippetExpression("1")
            ),
            new CodeMethodReturnStatement(new CodeSnippetExpression("1"))
        )
    );

    Calc.Statements.Add(
        new CodeMethodReturnStatement(
            new CodeBinaryOperatorExpression(
                new CodeMethodInvokeExpression(null, "Calc", 
                    new CodeBinaryOperatorExpression(
                        new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.Subtract,
                        new CodeSnippetExpression("1")
                    )
                ), CodeBinaryOperatorType.Add,
                new CodeMethodInvokeExpression(null, "Calc", 
                    new CodeBinaryOperatorExpression(
                        new CodeArgumentReferenceExpression("x"), CodeBinaryOperatorType.Subtract,
                        new CodeSnippetExpression("2")
                    )
                )
            )
        )
    );

    CodeTypeDeclaration Fibonacci = new CodeTypeDeclaration();
    Fibonacci.Members.Add(Calc);

    CodeNamespace ns = new CodeNamespace("TestNS");
    ns.Types.Add(Fibonacci);

    CodeCompileUnit unit = new CodeCompileUnit();
    unit.Namespaces.Add(ns);

    using (TextWriter tw = new IndentedTextWriter(Console.Out))
    {
        using (CodeDomProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider())
        {
            codeProvider.GenerateCodeFromCompileUnit(unit, tw, new CodeGeneratorOptions());
        }
    }
}
{code:c#}