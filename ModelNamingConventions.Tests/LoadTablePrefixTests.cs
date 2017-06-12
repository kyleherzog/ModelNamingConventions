using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelNamingConventions.Tests.Helpers;

namespace ModelNamingConventions.Tests
{
    [TestClass]
    public class LoadTablePrefixTests
    {
        private const string assemblySetTablePrefix = "Tbl";

        [TestMethod]
        public void LoadTablePrefixSetsStringEmptyWhenNoAssemblyAttribute()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            convention.LoadTablePrefix(AssemblyManager.GetDefault());
            Assert.AreEqual(string.Empty, convention.TablePrefix);
        }

        [TestMethod]
        public void LoadTablePrefixSetsValueWhenAssemblyAttributeSet()
        {
            var convention = ConventionGenerator.CreateModelNaming(ConventionGenerator.EmptyConfig);
            convention.LoadTablePrefix(AssemblyManager.LoadTestAttributesAssembly());
            Assert.AreEqual(assemblySetTablePrefix, convention.TablePrefix);
        }
    }
}