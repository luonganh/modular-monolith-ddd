using Mmdddshop.Modules.UserAccess.Domain.Users.Events;

namespace Mmdddshop.Modules.UserAccess.Domain.UnitTests.Users
{
    [TestFixture]
    public class UserTests : TestBase
    {
        [Test]
        public void CreateExternalUser_WithoutOptionalClaims_UsesNeutralDefaults()
        {
            var user = User.CreateExternalUser(Guid.NewGuid().ToString(), null, "jdoe", null, null, null, null);

            user.GetEmail().Should().BeNull();
            user.GetFirstName().Should().BeEmpty();
            user.GetLastName().Should().BeEmpty();
            user.GetName().Should().Be("jdoe");
        }

        [Test]
        public void SyncProfile_UpdatesFirstAndLastName()
        {
            var user = User.CreateExternalUser(Guid.NewGuid().ToString(), null, "jdoe", null, null, null, null);

            user.SyncProfile(null, "jdoe@example.com", "John Doe", "John", "Doe");

            user.GetEmail().Should().Be("jdoe@example.com");
            user.GetFirstName().Should().Be("John");
            user.GetLastName().Should().Be("Doe");
        }

        [Test]
        public void SyncProfile_WithOnlyTechnicalRoles_RemovesAllRolesAndRaisesProfileUpdated()
        {
            var user = User.CreateExternalUser(Guid.NewGuid().ToString(), ["Staff"], "staff1", null, null, null, null);
            user.ClearDomainEvents(); // bỏ UserCreatedDomainEvent để chỉ kiểm tra event của SyncProfile

            user.SyncProfile(["default-roles-mmdddshop", "offline_access"], null, null);

            user.GetRoles().Should().BeEmpty();
            DomainEventsTestHelper.GetAllDomainEvents(user)
                .Should().ContainSingle(e => e is UserProfileUpdatedDomainEvent);
        }

        [Test]
        public void SyncProfile_WithNullRoles_KeepsExistingRoles()
        {
            var user = User.CreateExternalUser(Guid.NewGuid().ToString(), ["Staff"], "staff1", null, null, null, null);
            user.ClearDomainEvents();

            user.SyncProfile(null, null, null);

            user.GetRoles().Select(r => r.Value).Should().BeEquivalentTo(["Staff"]);
            DomainEventsTestHelper.GetAllDomainEvents(user).Should().BeEmpty();
        }

        [Test]
        public void CreateExternalUser_FiltersUnknownRolesAndNormalizesCasing()
        {
            var user = User.CreateExternalUser(
                Guid.NewGuid().ToString(), ["staff", "Manager", "offline_access"], "jdoe", null, null, null, null);

            user.GetRoles().Select(r => r.Value).Should().BeEquivalentTo(["Staff", "Manager"]);
        }

        [Test]
        public void SyncProfile_WithSameRoles_DoesNotRaiseProfileUpdated()
        {
            var user = User.CreateExternalUser(Guid.NewGuid().ToString(), ["Staff"], "staff1", null, null, null, null);
            user.ClearDomainEvents();

            user.SyncProfile(["STAFF", "default-roles-mmdddshop"], null, null);

            user.GetRoles().Select(r => r.Value).Should().BeEquivalentTo(["Staff"]);
            DomainEventsTestHelper.GetAllDomainEvents(user).Should().BeEmpty();
        }
    }
}
