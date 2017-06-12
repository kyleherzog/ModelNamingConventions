using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class LoadColumnCasingStyleTests
    {
        private const CasingStyle defaultColumnCasing = CasingStyle.Snake;
        private const CasingStyle configColumnCasing = CasingStyle.Hungarian;
        private const CasingStyle assemblyColumnCasing = CasingStyle.Train;

        [TestMethod]
        public void ColumnCasingStyleSetToDefaultColumnCasingWhenNoConfigOrAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            convention.LoadColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(defaultColumnCasing, convention.ColumnCasingStyle);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToAssemblyValueWhenSetAlongWithDefaultsOnlyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            convention.LoadColumnCasingStyle(assembly);

            Assert.AreEqual(assemblyColumnCasing, convention.ColumnCasingStyle);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToConfigValueWhenSetAlongWithAssemblyAndDefaultsConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            convention.LoadColumnCasingStyle(assembly);

            Assert.AreEqual(configColumnCasing, convention.ColumnCasingStyle);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToConfigValueWhenWhenSetAndNoAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            convention.LoadColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(configColumnCasing, convention.ColumnCasingStyle);
        }

        [TestMethod]
        public void ColumnCasingStyleSetToNoneWhenNoAssemblyAttributesAndEmptyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            convention.LoadColumnCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(CasingStyle.None, convention.ColumnCasingStyle);
        }
    }
}