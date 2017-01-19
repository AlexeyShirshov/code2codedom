using LinqToCodedom.CodeDomPatterns;
using System;
using System.CodeDom;
using System.Linq;

namespace LinqToCodedom.Extensions
{
    public static class CodePropertyImplementsInterfaceExtensions
    {
        #region Property

        public static CodePropertyImplementsInterface Implements(this CodePropertyImplementsInterface property, CodeTypeReference t, string interfaceProperty)
        {
            if (!property.Property.ImplementationTypes.Cast<CodeTypeReference>().Any(it=>it.IsEquals(t)))
                property.Property.ImplementationTypes.Add(t);

            if (!string.IsNullOrEmpty(interfaceProperty) && !property.InterfaceProperties.Any(it=>it.Item2 == interfaceProperty && it.Item1.IsEquals(t)))
            {
                property.InterfaceProperties.Add(new Tuple<CodeTypeReference,string>(t, interfaceProperty));
            }
            return property;
        }

        #endregion
    }
}
