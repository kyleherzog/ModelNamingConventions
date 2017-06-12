using System.Configuration;

namespace ModelNamingConventions.Config
{
    public class AutoPrefixColumnConfigurationElement : ConfigurationElement
    {
        public AutoPrefixColumnConfigurationElement()
        {
        }

        public AutoPrefixColumnConfigurationElement(string name)
        {
            Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }
    }
}