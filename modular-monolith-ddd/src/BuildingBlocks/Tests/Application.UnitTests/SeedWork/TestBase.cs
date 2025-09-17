// File: BuildingBlocks/Tests/Application.UnitTests/SeedWork/TestBase.cs
// Summary: Lightweight base for BuildingBlocks application unit tests.
namespace ModularMonolithDDD.BuildingBlocks.Tests.Application.UnitTests.SeedWork
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
