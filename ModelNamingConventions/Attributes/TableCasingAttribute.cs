using System;
using CodeCasing;

namespace ModelNamingConventions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class TableCasingAttribute : Attribute
    {
        private readonly CasingStyle myStyle;

        public TableCasingAttribute(CasingStyle style)
        {
            myStyle = style;
        }

        public CasingStyle Style
        {
            get
            {
                return myStyle;
            }
        }
    }
}