using LinqToCodedom.CodeDomPatterns;
using System;
using System.CodeDom;

namespace LinqToCodedom.Extensions
{
    public static class CodePropertyImplementsInterfaceExtensions
    {
        #region Property

        public static CodePropertyImplementsInterface Implements(this CodePropertyImplementsInterface property, CodeTypeReference t, string interfaceProperty)
        {
            //if ((property.Attributes & MemberAttributes.Private) == MemberAttributes.Private)
            //    property.Property.PrivateImplementationType = t;
            //else
            {
                if (!property.Property.ImplementationTypes.Contains(t))
                    property.Property.ImplementationTypes.Add(t);

                if (!string.IsNullOrEmpty(interfaceProperty))
                    property.InterfaceProperties[t] = interfaceProperty;
            }
            return property;
        }

        #endregion
    }
}
