// File: Modules/UserAccess/Tests/ArchTests/Module/ModuleTests.cs
// Purpose: Cross-layer and cross-module architectural checks for UserAccess.
// Ensures infra references app/domain, not vice versa; and no leakage
// to other business modules.
namespace ModularMonolithDDD.Modules.UserAccess.Tests.ArchTests.Module
{
    [TestFixture]
    public class ModuleTests : TestBase
    {
        [Test]
        public void UserAccessModule_ShouldNotHaveDependency_ToOtherModules()
        {
            var otherModules = new[]
            {
                "ModularMonolithDDD.Modules.Registrations",
                "ModularMonolithDDD.Modules.Products", 
                "ModularMonolithDDD.Modules.Orders"
            };

            var result = Types.InAssemblies(new[] { ApplicationAssembly, DomainAssembly, InfrastructureAssembly })
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess")
                .Should()
                .NotHaveDependencyOnAny(otherModules)
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Infrastructure_ShouldHaveDependency_ToApplication()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess.Infrastructure")
                .Should()
                .HaveDependencyOn("ModularMonolithDDD.Modules.UserAccess.Application")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Infrastructure_ShouldHaveDependency_ToDomain()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess.Infrastructure")
                .Should()
                .HaveDependencyOn("ModularMonolithDDD.Modules.UserAccess.Domain")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Controllers_ShouldNotHaveDependency_ToInfrastructure()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess.Infrastructure.Controllers")
                .Should()
                .NotHaveDependencyOnAny("ModularMonolithDDD.Modules.UserAccess.Infrastructure.DataAccess",
                                       "ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration")
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
