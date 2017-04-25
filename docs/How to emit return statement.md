Below statement declares parameterless method Print with return value of type int and returns 100
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(int), ()=>"Print",
   Emit.@return(()=>100)
)
{code:c#}
Another example
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), ()=>"Print",
   Emit.declare("guid", ()=>Guid.NewGuid()),
   Emit.@return((Guid guid)=>guid.ToString())
)
{code:c#}
And here is what we get from CodeDOM
{code:c#}
public static string Print() {
    System.Guid guid = System.Guid.NewGuid();
    return guid.ToString();
}
{code:c#}