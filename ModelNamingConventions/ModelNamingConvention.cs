using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
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
            : this(null as Configuration)
        {
        }

        internal ModelNamingConvention(Configuration config)
        {
            if (config == null)
            {
                if (HttpContext.Current == null)
                {
                    Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                else
                {
                    Config = WebConfigurationManager.OpenWebConfiguration("~");
                }
            }
            else
            {
                Config = config;
            }

            this.Types().Configure(ConfigureTypes());
            this.Properties().Configure(ConfigureProperties());
        }

        private Dictionary<string, IEnumerable<string>> autoPrefixedColumns;

        internal IEnumerable<string> GetAutoPrefixedColumns(Assembly assembly)
        {
            if (autoPrefixedColumns == null)
            {
                autoPrefixedColumns = new Dictionary<string, IEnumerable<string>>();
            }

            if (!autoPrefixedColumns.Keys.Contains(assembly.FullName))
            {
                var columns = new List<string>();

                if (ConfigSection != null)
                {
                    if (!ConfigSection.IsReplacingAutoPrefixColumns)
                    {
                        var attribute = assembly.GetCustomAttributes(typeof(ColumnAutoPrefixingAttribute), false).Cast<ColumnAutoPrefixingAttribute>().FirstOrDefault();
                        if (attribute != null)
                        {
                            columns.AddRange(attribute.PropertyNames);
                        }
                    }

                    foreach (AutoPrefixColumnConfigurationElement item in ConfigSection.AutoPrefixColumns)
                    {
                        columns.Add(item.Name);
                    }
                }

                autoPrefixedColumns[assembly.FullName] = columns;
            }

            return autoPrefixedColumns[assembly.FullName];
        }

        private Dictionary<string, CasingStyle> columnCasingStyles;

        internal CasingStyle GetColumnCasingStyle(Assembly assembly)
        {
            if (columnCasingStyles == null)
            {
                columnCasingStyles = new Dictionary<string, CasingStyle>();
            }

            if (!columnCasingStyles.Keys.Contains(assembly.FullName))
            {
                CasingStyle? style;

                if (ConfigSection != null && ConfigSection.ColumnCasing.HasValue)
                {
                    style = ConfigSection.ColumnCasing;
                }
                else
                {
                    style = GetAssemblyColumnCasingStyle(assembly);
                    if (!style.HasValue && ConfigSection != null)
                    {
                        style = ConfigSection.ColumnCasingDefault;
                    }

                    if (!style.HasValue)
                    {
                        style = CasingStyle.None;
                    }
                }

                columnCasingStyles[assembly.FullName] = style.Value;
            }

            return columnCasingStyles[assembly.FullName];
        }

        internal Configuration Config { get; }

        private Dictionary<string, CasingStyle> tableCasingStyles;

        internal CasingStyle GetTableCasingStyle(Assembly assembly)
        {
            if (tableCasingStyles == null)
            {
                tableCasingStyles = new Dictionary<string, CasingStyle>();
            }

            if (!tableCasingStyles.Keys.Contains(assembly.FullName))
            {
                CasingStyle? style;

                if (ConfigSection != null && ConfigSection.TableCasing.HasValue)
                {
                    style = ConfigSection.TableCasing;
                }
                else
                {
                    style = GetAssemblyTableCasingStyle(assembly);
                    if (!style.HasValue && ConfigSection != null)
                    {
                        style = ConfigSection.TableCasingDefault;
                    }

                    if (!style.HasValue)
                    {
                        style = CasingStyle.None;
                    }
                }

                tableCasingStyles[assembly.FullName] = style.Value;
            }

            return tableCasingStyles[assembly.FullName];
        }

        private Dictionary<string, string> tablePrefixes;

        internal string GetTablePrefix(Assembly assembly)
        {
            if (tablePrefixes == null)
            {
                tablePrefixes = new Dictionary<string, string>();
            }

            if (!tablePrefixes.Keys.Contains(assembly.FullName))
            {
                var attribute = assembly.GetCustomAttributes(typeof(TablePrefixAttribute), false).Cast<TablePrefixAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    tablePrefixes[assembly.FullName] = attribute.Value;
                }
                else
                {
                    tablePrefixes[assembly.FullName] = string.Empty;
                }
            }

            return tablePrefixes[assembly.FullName];
        }

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
            var style = GetColumnCasingStyle(assembly);

            if (style > CasingStyle.None)
            {
                result = CasingConverter.Convert(result, style);
            }

            return result;
        }

        internal string ApplyTableNaming(Assembly assembly, string typeName)
        {
            var tableName = string.Concat(GetTablePrefix(assembly), typeName);

            var style = GetTableCasingStyle(assembly);
            if (style > CasingStyle.None)
            {
                tableName = CasingConverter.Convert(tableName, style);
            }

            return tableName;
        }

        private string ApplyColumnPrefixing(string columnName, string declaringTypeName, Assembly assembly)
        {
            var columns = GetAutoPrefixedColumns(assembly);
            if (columns.Contains(columnName))
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