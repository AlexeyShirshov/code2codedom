Method parameters is declaring as lambda parameters
Below statement declare parameterless method called Print.
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, ()=>"Print",...
{code:c#}
Below statement declare method Print with two parameters - i type of int and s type of string.
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int i, string s)=>"Print",...
{code:c#}
Here is we print the value of those parameters.
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int i, string s)=>"Print",
   Emit.stmt((int i, string s)=>Console.WriteLine("i={0}, s={1}", i, s))
)
{code:c#}
In above example stmt method accept lambda function with two params - i and s, which is reference to Print method parameters.