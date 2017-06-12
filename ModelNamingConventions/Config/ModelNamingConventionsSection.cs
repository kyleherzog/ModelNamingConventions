using System.Configuration;
using CodeCasing;

namespace ModelNamingConventions.Config
{
    public class ModelNamingConventionsSection : ConfigurationSection
    {
        [ConfigurationProperty("autoPrefixColumns", IsDefaultCollection = false)]
        [ConfigurationCollection(
            typeof(AutoPrefixColumnConfigurationElement),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public AutoPrefixColumnsCollection AutoPrefixColumns
        {
            get
            {
                return (AutoPrefixColumnsCollection)this["autoPrefixColumns"];
            }
        }

        [ConfigurationProperty("columnCasing", IsRequired = false)]
        public CasingStyle? ColumnCasing
        {
            get
            {
                return (CasingStyle?)this["columnCasing"];
            }

            set
            {
                this["columnCasing"] = value;
            }
        }

        [ConfigurationProperty("columnCasingDefault", IsRequired = false)]
        public CasingStyle? ColumnCasingDefault
        {
            get
            {
                return (CasingStyle?)this["columnCasingDefault"];
            }

            set
            {
                this["columnCasingDefault"] = value;
            }
        }

        [ConfigurationProperty("isReplacingAutoPrefixColumns", DefaultValue = false, IsRequired = false)]
        public bool IsReplacingAutoPrefixColumns
        {
            get
            {
                return (bool)this["isReplacingAutoPrefixColumns"];
            }
        }

        [ConfigurationProperty("tableCasing", IsRequired = false)]
        public CasingStyle? TableCasing
        {
            get
            {
                return (CasingStyle?)this["tableCasing"];
            }

            set
            {
                this["tableCasing"] = value;
            }
        }

        [ConfigurationProperty("tableCasingDefault", IsRequired = false)]
        public CasingStyle? TableCasingDefault
        {
            get
            {
                return (CasingStyle?)this["tableCasingDefault"];
            }

            set
            {
                this["tableCasingDefault"] = value;
            }
        }
    }
}