// File: Tests/ArchTests/Modules/ModuleTests.cs
// Purpose: Enforce module boundaries for `UserAccess`.
// Verifies module does not reference other modules and
// domain/application layers don't depend on infrastructure.
namespace ModularMonolithDDD.Tests.ArchTests.Modules
{
    [TestFixture]
    public class ModuleTests : TestBase
    {
        [Test]
        public void UserAccessModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            // Currently only UserAccess module exists
            // This test ensures UserAccess module doesn't have dependencies to other modules
            List<string> otherModules = ["ModularMonolithDDD.Modules.Registrations",
                                      "ModularMonolithDDD.Modules.Products",
                                      "ModularMonolithDDD.Modules.Orders"];
            
            List<Assembly> userAccessAssemblies =
            [
                typeof(IUserAccessModule).Assembly,
                typeof(User).Assembly,
                typeof(UserAccessContext).Assembly
            ];

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(INotificationHandler<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
                .And().DoNotHaveName("EventsBusStartup")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Domain_ShouldNotHaveDependency_ToInfrastructure()
        {
            var result = Types.InAssembly(typeof(User).Assembly)
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess.Domain")
                .Should()
                .NotHaveDependencyOnAny("ModularMonolithDDD.Modules.UserAccess.Infrastructure")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void Application_ShouldNotHaveDependency_ToInfrastructure()
        {
            var result = Types.InAssembly(typeof(IUserAccessModule).Assembly)
                .That()
                .ResideInNamespace("ModularMonolithDDD.Modules.UserAccess.Application")
                .Should()
                .NotHaveDependencyOnAny("ModularMonolithDDD.Modules.UserAccess.Infrastructure")
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
