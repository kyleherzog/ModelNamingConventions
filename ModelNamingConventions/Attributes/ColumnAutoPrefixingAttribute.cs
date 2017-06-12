using System;
using System.Collections.Generic;

namespace ModelNamingConventions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ColumnAutoPrefixingAttribute : Attribute
    {
        public ColumnAutoPrefixingAttribute(string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        public IEnumerable<string> PropertyNames { get; }
    }
}