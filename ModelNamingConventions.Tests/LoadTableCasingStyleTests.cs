using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class LoadTableCasingStyleTests
    {
        private const CasingStyle defaultTableCasing = CasingStyle.ScreamingSnake;
        private const CasingStyle configTableCasing = CasingStyle.Pascal;
        private const CasingStyle assemblyTableCasing = CasingStyle.Spinal;

        [TestMethod]
        public void TableCasingStyleSetToDefaultTableCasingWhenNoConfigOrAssemblyCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            convention.LoadTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(defaultTableCasing, convention.TableCasingStyle);
        }

        [TestMethod]
        public void TableCasingStyleSetToAssemblyValueWhenSetAlongWithDefaultsOnlyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            convention.LoadTableCasingStyle(assembly);

            Assert.AreEqual(assemblyTableCasing, convention.TableCasingStyle);
        }

        [TestMethod]
        public void TableCasingStyleSetToConfigValueWhenSetAlongWithAssemblyAndDefaultsConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            var assembly = AssemblyManager.LoadTestAttributesAssembly();
            convention.LoadTableCasingStyle(assembly);

            Assert.AreEqual(configTableCasing, convention.TableCasingStyle);
        }

        [TestMethod]
        public void TableCasingStyleSetToConfigValueWhenSetAndNoAssemblyAttributesSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.CasingPlusDefaultsConfig);
            convention.LoadTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(configTableCasing, convention.TableCasingStyle);
        }

        [TestMethod]
        public void TableCasingStyleSetToNoneWhenNoAssemblyAttributesAndEmptyConfig()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            convention.LoadTableCasingStyle(AssemblyManager.GetDefault());

            Assert.AreEqual(CasingStyle.None, convention.TableCasingStyle);
        }
    }
}