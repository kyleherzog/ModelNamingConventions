using CodeCasing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class ApplyTableNamingTests
    {
        private const string testTypeName = "MyWonderfulWidget";
        private const string assemblySetTablePrefix = "Tbl";

        [TestMethod]
        public void ApplyTableNamingReturnsTestTypeNameWhenNoTablePrefixOrCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            var tableName = convention.ApplyTableNaming(AssemblyManager.GetDefault(), testTypeName);
            Assert.AreEqual(testTypeName, tableName);
        }

        [TestMethod]
        public void ApplyTableNamingReturnsCasedTestTypeNameWhenNoTablePrefixButCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var tableName = convention.ApplyTableNaming(AssemblyManager.GetDefault(), testTypeName);
            Assert.AreEqual(testTypeName.ToScreamingSnakeCase(), tableName);
        }

        [TestMethod]
        public void ApplyTableNamingReturnsPrefixedAndCasedTestTypeNameWhenTablePrefixAndCasingSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var tableName = convention.ApplyTableNaming(AssemblyManager.LoadTestAttributesAssembly(), testTypeName);
            var expected = string.Concat(assemblySetTablePrefix, testTypeName).ToSpinalCase();
            Assert.AreEqual(expected, tableName);
        }

        [TestMethod]
        public void ApplyTableNamingReturnsProperlyPrefixedTablesAcrossMultipleAssemblies()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.DefaultsOnlyConfig);
            var tableName = convention.ApplyTableNaming(AssemblyManager.GetDefault(), testTypeName);
            Assert.AreEqual(testTypeName.ToScreamingSnakeCase(), tableName);

            tableName = convention.ApplyTableNaming(AssemblyManager.LoadTestAttributesAssembly(), testTypeName);
            var expected = string.Concat(assemblySetTablePrefix, testTypeName).ToSpinalCase();
            Assert.AreEqual(expected, tableName);
        }
    }
}