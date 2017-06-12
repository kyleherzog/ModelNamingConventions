using System.Reflection;

namespace ModelNamingConventions.Tests.Helpers
{
    public class AssemblyManager
    {
        private const string testAttributesAssemblyName = "ModelNamingConventions.TestAssemblyAttributes";

        public static Assembly LoadTestAttributesAssembly()
        {
            return Assembly.Load(testAttributesAssemblyName);
        }

        public static Assembly GetDefault()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}