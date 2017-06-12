using System;

namespace ModelNamingConventions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class TablePrefixAttribute : Attribute
    {
        private readonly string myValue;

        public TablePrefixAttribute(string value)
        {
            myValue = value;
        }

        public string Value
        {
            get
            {
                return myValue;
            }
        }
    }
}