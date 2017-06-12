using System;
using CodeCasing;

namespace ModelNamingConventions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ColumnCasingAttribute : Attribute
    {
        private readonly CasingStyle myStyle;

        public ColumnCasingAttribute(CasingStyle style)
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