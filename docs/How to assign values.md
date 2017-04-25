Below statement declare variable i type of int and assign it value 100
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, ()=>"Print",
   Emit.declare(typeof(int), "i"),
   Emit.assignVar("i", ()=>100)
)
{code:c#}
Below statement declare variable i type of int and assign it value 100 plus the value of parameter j
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, (int j)=>"Print",
   Emit.declare(typeof(int), "i"),
   Emit.assignVar("i", (int j)=>100+j)
)
{code:c#}