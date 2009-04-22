using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;

namespace LinqToCodedom.Visitors
{
    public class VisitorContext
    {
        private List<CodeExpression> _params = new List<CodeExpression>();

        public List<CodeExpression> Params
        {
            get { return _params; }
        }

        public CodeExpression VisitParameter(ParameterExpression parameterExpression)
        {
            return _params.Find((p) => p.UserData["name"] == parameterExpression.Name);
        }

        public void VisitParams(System.Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> @params)
        {
            foreach (var p in @params)
            {
                if (p.Type.IsGenericType)
                {
                    if (p.Type.GetGenericTypeDefinition() == typeof(Par<>))
                        _params.Add(new CodeArgumentReferenceExpression(p.Name));
                    else if (p.Type.GetGenericTypeDefinition() == typeof(Var<>))
                        _params.Add(new CodeVariableReferenceExpression(p.Name));
                    else
                        throw new NotImplementedException();

                    _params[_params.Count - 1].UserData["name"] = p.Name;
                }
            }
        }

    }
}
