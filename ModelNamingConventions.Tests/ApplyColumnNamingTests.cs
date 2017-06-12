using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class ApplyColumnNamingTests
    {
        private const string prefixedColumnName = "Key";
        private const string baseColumnName = "LongDescription";
        private const string declaringTypeName = "MobileDevice";

        [TestMethod]
        public void ApplyColumnNamingReturnsColumnNameWhenNoCasingSetAndNoPrefixMatchFound()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.PrefixingOnlyConfig);
            var columnName = convention.ApplyColumnNaming(baseColumnName, declaringTypeName, AssemblyManager.GetDefault());
            Assert.AreEqual(baseColumnName, columnName);
        }

        [TestMethod]
        public void ApplyColumnNamingReturnsColumnNameCasedWhenCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var columnName = convention.ApplyColumnNaming(baseColumnName, declaringTypeName, AssemblyManager.GetDefault());
            var expected = baseColumnName.ToSnakeCase();
            Assert.AreEqual(expected, columnName);
        }

        [TestMethod]
        public void ApplyColumnNamingReturnsColumnNamePrefixedWhenPrefixingMatchFound()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.PrefixingOnlyConfig);
            var columnName = convention.ApplyColumnNaming(prefixedColumnName, declaringTypeName, AssemblyManager.GetDefault());
            var expected = string.Concat(declaringTypeName, prefixedColumnName);
            Assert.AreEqual(expected, columnName);
        }

        [TestMethod]
        public void ApplyColumnNamingReturnsColumnNamePrefixedAndCasedWhenPrefixingMatchFoundAndCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.PrefixingOnlyConfig);
            var columnName = convention.ApplyColumnNaming(prefixedColumnName, declaringTypeName, AssemblyManager.LoadTestAttributesAssembly());
            var expected = string.Concat(declaringTypeName, prefixedColumnName).ToTrainCase();
            Assert.AreEqual(expected, columnName);
        }
    }
}