using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class GetTableCasingStyleTests
    {
        private const CasingStyle defaultTableCasing = CasingStyle.ScreamingSnake;
        private const CasingStyle configTableCasing = CasingStyle.Pascal;
        private const CasingStyle assemblyTableCasing = CasingStyle.Spinal;

        [TestMethod]
        public void TableCasingStyleSetToDefaultTableCasingWhenNoConfigOrAssemblyCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var style = convention.GetTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(defaultTableCasing, style);
        }

        [TestMethod]
        public void TableCasingStyleSetToAssemblyValueWhenSetAlongWithDefaultsOnlyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            var style = convention.GetTableCasingStyle(assembly);

            Assert.AreEqual(assemblyTableCasing, style);
        }

        [TestMethod]
        public void TableCasingStyleSetToConfigValueWhenSetAlongWithAssemblyAndDefaultsConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            var style = convention.GetTableCasingStyle(assembly);

            Assert.AreEqual(configTableCasing, style);
        }

        [TestMethod]
        public void TableCasingStyleSetToConfigValueWhenSetAndNoAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var style = convention.GetTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(configTableCasing, style);
        }

        [TestMethod]
        public void TableCasingStyleSetToNoneWhenNoAssemblyAttributesAndEmptyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            var style = convention.GetTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(CasingStyle.None, style);
        }
    }
}