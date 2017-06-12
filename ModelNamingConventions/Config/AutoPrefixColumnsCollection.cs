using System;
using System.Configuration;

namespace ModelNamingConventions.Config
{
    [ConfigurationCollection(typeof(AutoPrefixColumnConfigurationElement))]
    public class AutoPrefixColumnsCollection : ConfigurationElementCollection
    {
        public AutoPrefixColumnConfigurationElement this[int index]
        {
            get
            {
                return (AutoPrefixColumnConfigurationElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(AutoPrefixColumnConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(AutoPrefixColumnConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AutoPrefixColumnConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AutoPrefixColumnConfigurationElement)element).Name;
        }
    }
}