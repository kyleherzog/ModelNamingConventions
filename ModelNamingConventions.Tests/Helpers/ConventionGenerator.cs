using System.Configuration;

namespace ModelNamingConventions.Tests.Helpers
{
    public class ConventionGenerator
    {
        public const string EmptyConfig = "Empty";
        public const string DefaultsOnlyConfig = "DefaultsOnly";
        public const string PrefixingOnlyConfig = "PrefixingOnly";
        public const string CasingPlusDefaultsConfig = "CasingPlusDefaults";

        public static ModelNamingConvention CreateModelNaming(string configIdentifier)
        {
            {
                var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = $"Config\\{configIdentifier}App.config" };
                var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                return new ModelNamingConvention(config);
            }
        }
    }
}