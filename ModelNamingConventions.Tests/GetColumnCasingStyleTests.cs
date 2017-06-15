using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class GetColumnCasingStyleTests
    {
        private const CasingStyle defaultColumnCasing = CasingStyle.Snake;
        private const CasingStyle configColumnCasing = CasingStyle.Hungarian;
        private const CasingStyle assemblyColumnCasing = CasingStyle.Train;

        [TestMethod]
        public void ColumnCasingStyleSetToDefaultColumnCasingWhenNoConfigOrAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var style = convention.GetColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(defaultColumnCasing, style);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToAssemblyValueWhenSetAlongWithDefaultsOnlyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            var style = convention.GetColumnCasingStyle(assembly);

            Assert.AreEqual(assemblyColumnCasing, style);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToConfigValueWhenSetAlongWithAssemblyAndDefaultsConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            var style = convention.GetColumnCasingStyle(assembly);

            Assert.AreEqual(configColumnCasing, style);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToConfigValueWhenWhenSetAndNoAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var style = convention.GetColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(configColumnCasing, style);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToNoneWhenNoAssemblyAttributesAndEmptyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            var style = convention.GetColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(CasingStyle.None, style);
        }
    }
}