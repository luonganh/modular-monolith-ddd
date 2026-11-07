// File: Tests/ArchTests/Api/ApiTests.cs
// Purpose: Architectural checks for the public API surface.
// Ensures API layer respects module boundaries and does not
// depend on lower-level infrastructure directly.
namespace Mmdddshop.Tests.ArchTests.Api
{
    [TestFixture]
    public class ApiTests : TestBase
    {
        [Test]
        public void UserAccessApi_DoesNotHaveDependency_ToOtherModules()
        {
            // Currently only UserAccess module exists
            // This test ensures UserAccess API doesn't have dependencies to other modules
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Mmdddshop.API.Modules.UserAccess")
                .Should()
                .NotHaveDependencyOnAny("Mmdddshop.Modules.Registrations",
                                      "Mmdddshop.Modules.Products",
                                      "Mmdddshop.Modules.Orders")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Controllers_ShouldNotHaveDependency_ToInfrastructure()
        {
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Mmdddshop.API.Controllers")
                .Should()
                .NotHaveDependencyOnAny("Mmdddshop.Modules.UserAccess.Infrastructure")
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
