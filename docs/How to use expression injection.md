Expression injection allows create complex CodeDom expressions in simple way like String.Format simplifies string construction. 
{code:c#}
   CodeExpression exp = CodeDom.GetExpression(() => 1);

   CodeExpression exp2 = CodeDom.GetExpression((int g) => CodeDom.InjectExp<int>(0) + g, exp);
{code:c#}