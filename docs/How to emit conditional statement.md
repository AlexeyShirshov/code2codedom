Below statement emit conditional statement
{code:c#}
AddMethod(MemberAttributes.Public | MemberAttributes.Static, typeof(string), (int i)=>"Print",
   Emit.@if((int i)=>i < 10,
      Emit.@return(()=>"i less than 10")
   ),
   Emit.@return(()=>"i greater than or equals 10")
)
{code:c#}
Generated code is as following
{code:c#}
public static string Print(int i) {
    if ((i < 10)) {
        return "i less than 10";
    }
    return "i greater than or equals 10";
}
{code:c#}