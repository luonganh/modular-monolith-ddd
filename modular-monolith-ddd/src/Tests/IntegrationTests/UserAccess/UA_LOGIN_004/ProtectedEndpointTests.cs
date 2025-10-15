using System.Net;
using NUnit.Framework;

namespace ModularMonolithDDD.Tests.IntegrationTests.UserAccess.UA_LOGIN_004
{
    [TestFixture]
    public class ProtectedEndpointTests
    {
        private ApiFactory _factory = null!;

        [SetUp]
        public void SetUp() => _factory = new ApiFactory();

        [TearDown]
        public void TearDown() => _factory.Dispose();

        [Test]
        public async Task Get_Ping_Unauthenticated_Returns_401()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/api/Protected/ping");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Get_Ping_Authenticated_MissingScope_Returns_403()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Auth", "user");
            client.DefaultRequestHeaders.Add("X-Test-Name", "tester");

            var resp = await client.GetAsync("/api/Protected/ping");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task Get_Ping_Authenticated_WithScope_Returns_200()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Test-Auth", "user");
            client.DefaultRequestHeaders.Add("X-Test-Name", "tester");
            client.DefaultRequestHeaders.Add("X-Test-Scopes", "modular-monolith-ddd-api");

            var resp = await client.GetAsync("/api/Protected/ping");
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}