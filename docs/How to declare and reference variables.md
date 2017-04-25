Below statement declare variable i of type int and initialize it with value 10
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, ()=>"Print",
   Emit.declare("i", ()=>10)
)
{code:c#}
Below statement declare variable i of type int, initialize it with value 10 plus j parameter value and print the value to console
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int j)=>"Print",
   Emit.declare("i", (int j)=>10+j),
   Emit.stmt((int i)=>Console.WriteLine(i))
)
{code:c#}