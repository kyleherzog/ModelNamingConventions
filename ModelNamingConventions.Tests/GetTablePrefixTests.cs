using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class GetTablePrefixTests
    {
        private const string assemblySetTablePrefix = "Tbl";

        [TestMethod]
        public void LoadTablePrefixSetsStringEmptyWhenNoAssemblyAttribute()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            var prefix = convention.GetTablePrefix(AssemblyManager.GetDefault());
            Assert.AreEqual(string.Empty, prefix);
        }

        [TestMethod]
        public void LoadTablePrefixSetsValueWhenAssemblyAttributeSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            var prefix = convention.GetTablePrefix(AssemblyManager.LoadTestAttributesAssembly());
            Assert.AreEqual(assemblySetTablePrefix, prefix);
        }
    }
}