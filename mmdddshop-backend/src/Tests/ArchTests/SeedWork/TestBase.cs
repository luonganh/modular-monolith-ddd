// File: Tests/ArchTests/SeedWork/TestBase.cs
// Summary: Shared helpers and constants for architecture tests.
// - Exposes `ApiAssembly` for arch scans
// - Provides small assertion helpers to keep tests concise
namespace ModularMonolithDDD.Tests.ArchTests.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApiAssembly => typeof(Program).Assembly;

        public const string UserAccessNamespace = "ModularMonolithDDD.Modules.UserAccess";

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            List<Type> failingTypes = [];
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
