using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using CodeCasing;
using ModelNamingConventions.Attributes;
using ModelNamingConventions.Config;

namespace ModelNamingConventions
{
    public class ModelNamingConvention : Convention
    {
        private ModelNamingConventionsSection _configSection;
        private bool hasLoadedConfig;

        public ModelNamingConvention()
            : this(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
        {
        }

        internal ModelNamingConvention(Configuration config)
        {
            Config = config;
            this.Types().Configure(ConfigureTypes());

            this.Properties().Configure(ConfigureProperties());
        }

        internal IEnumerable<string> AutoPrefixedColumns { get; set; }

        internal CasingStyle? ColumnCasingStyle { get; set; }

        internal Configuration Config { get; }

        internal CasingStyle? TableCasingStyle { get; set; }

        internal string TablePrefix { get; set; }

        protected ModelNamingConventionsSection ConfigSection
        {
            get
            {
                if (!hasLoadedConfig)
                {
                    _configSection = Config.GetSection("modelNamingConventions") as ModelNamingConventionsSection;
                    hasLoadedConfig = true;
                }

                return _configSection;
            }
        }

        internal string ApplyColumnNaming(string columnName, string declaringTypeName, Assembly assembly)
        {
            var result = ApplyColumnPrefixing(columnName, declaringTypeName, assembly);

            if (!ColumnCasingStyle.HasValue)
            {
                LoadColumnCasingStyle(assembly);
            }

            if (ColumnCasingStyle.Value > CasingStyle.None)
            {
                result = CasingConverter.Convert(result, ColumnCasingStyle.Value);
            }

            return result;
        }

        internal string ApplyTableNaming(Assembly assembly, string typeName)
        {
            if (TablePrefix == null)
            {
                LoadTablePrefix(assembly);
            }

            var tableName = string.Concat(TablePrefix, typeName);

            if (!TableCasingStyle.HasValue)
            {
                LoadTableCasingStyle(assembly);
            }

            if (TableCasingStyle.Value > CasingStyle.None)
            {
                tableName = CasingConverter.Convert(tableName, TableCasingStyle.Value);
            }

            return tableName;
        }

        internal void LoadAutoPrefixedColumns(Assembly assembly)
        {
            var columnsNames = new List<string>();

            if (ConfigSection != null)
            {
                if (!ConfigSection.IsReplacingAutoPrefixColumns)
                {
                    var attribute = assembly.GetCustomAttributes(typeof(ColumnAutoPrefixingAttribute), false).Cast<ColumnAutoPrefixingAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        columnsNames.AddRange(attribute.PropertyNames);
                    }
                }

                foreach (AutoPrefixColumnConfigurationElement item in ConfigSection.AutoPrefixColumns)
                {
                    columnsNames.Add(item.Name);
                }
            }

            AutoPrefixedColumns = columnsNames;
        }

        internal void LoadColumnCasingStyle(Assembly assembly)
        {
            if (ConfigSection != null && ConfigSection.ColumnCasing.HasValue)
            {
                ColumnCasingStyle = ConfigSection.ColumnCasing;
            }
            else
            {
                ColumnCasingStyle = GetAssemblyColumnCasingStyle(assembly);
                if (!ColumnCasingStyle.HasValue && ConfigSection != null)
                {
                    ColumnCasingStyle = ConfigSection.ColumnCasingDefault;
                }

                if (!ColumnCasingStyle.HasValue)
                {
                    ColumnCasingStyle = CasingStyle.None;
                }
            }
        }

        internal void LoadTableCasingStyle(Assembly assembly)
        {
            if (ConfigSection != null && ConfigSection.TableCasing.HasValue)
            {
                TableCasingStyle = ConfigSection.TableCasing;
            }
            else
            {
                TableCasingStyle = GetAssemblyTableCasingStyle(assembly);
                if (!TableCasingStyle.HasValue && ConfigSection != null)
                {
                    TableCasingStyle = ConfigSection.TableCasingDefault;
                }

                if (!TableCasingStyle.HasValue)
                {
                    TableCasingStyle = CasingStyle.None;
                }
            }
        }

        internal void LoadTablePrefix(Assembly assembly)
        {
            var attribute = assembly.GetCustomAttributes(typeof(TablePrefixAttribute), false).Cast<TablePrefixAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                TablePrefix = attribute.Value;
            }
            else
            {
                TablePrefix = string.Empty;
            }
        }

        private string ApplyColumnPrefixing(string columnName, string declaringTypeName, Assembly assembly)
        {
            if (AutoPrefixedColumns == null)
            {
                LoadAutoPrefixedColumns(assembly);
            }

            if (AutoPrefixedColumns.Contains(columnName))
            {
                columnName = string.Concat(declaringTypeName, columnName);
            }

            return columnName;
        }

        private System.Action<ConventionPrimitivePropertyConfiguration> ConfigureProperties()
        {
            return c =>
            {
                var columnName = c.ClrPropertyInfo.Name;
                var assembly = c.ClrPropertyInfo.DeclaringType.Assembly;
                var declaringTypeName = c.ClrPropertyInfo.DeclaringType.Name;

                var modifiedColumnName = ApplyColumnNaming(columnName, declaringTypeName, assembly);

                c.HasColumnName(modifiedColumnName);
            };
        }

        private System.Action<ConventionTypeConfiguration> ConfigureTypes()
        {
            return c =>
            {
                var tableName = ApplyTableNaming(c.ClrType.Assembly, c.ClrType.Name);

                c.ToTable(tableName);
            };
        }

        private CasingStyle? GetAssemblyColumnCasingStyle(Assembly assembly)
        {
            CasingStyle? style = null;
            var attribute = assembly.GetCustomAttributes(typeof(ColumnCasingAttribute), false).Cast<ColumnCasingAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                style = attribute.Style;
            }

            return style;
        }

        private CasingStyle? GetAssemblyTableCasingStyle(Assembly assembly)
        {
            CasingStyle? style = null;
            var attribute = assembly.GetCustomAttributes(typeof(TableCasingAttribute), false).Cast<TableCasingAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                style = attribute.Style;
            }

            return style;
        }
    }
}