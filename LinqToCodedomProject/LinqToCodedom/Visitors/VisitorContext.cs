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
        private List<CodeArgumentReferenceExpression> _params = new List<CodeArgumentReferenceExpression>();

        public List<CodeArgumentReferenceExpression> Params
        {
            get { return _params; }
        }

        public CodeExpression VisitParameter(ParameterExpression parameterExpression)
        {
            return _params.Find((p) => p.ParameterName == parameterExpression.Name);
        }

        public void VisitParams(System.Collections.ObjectModel.ReadOnlyCollection<ParameterExpression> @params)
        {
            foreach (var p in @params)
            {
                if (p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(Par<>))
                {
                    _params.Add(new CodeArgumentReferenceExpression(p.Name));
                }
            }
        }

    }
}
