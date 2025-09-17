// File: Modules/UserAccess/Tests/UnitTests/SeedWork/TestBase.cs
// Summary: Minimal base class for UserAccess unit tests. Add common
// setup/teardown when unit tests require shared helpers.
namespace ModularMonolithDDD.Modules.UserAccess.Tests.UnitTests.SeedWork
{
    public abstract class TestBase
    {
        [SetUp]
        public virtual void BeforeEachTest()
        {
            // Setup for unit tests
        }

        [TearDown]
        public virtual void AfterEachTest()
        {
            // Cleanup for unit tests
        }
    }
}
