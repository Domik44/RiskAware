using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class AccountTests : ServerTestsBase
    {
        private const string Endpoint = "api/Account";
        private readonly ITestOutputHelper _testOutputHelper;

        public AccountTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture) : base(
            testOutputHelper, fixture)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Login_Basic_User_is_OK()
        {
            StringContent content =
                new(
                    $"{{\"email\": \"{UserSeeds.BasicUser.Email}\",\"password\": \"{UserSeeds.BasicLogin.Password}\",\"rememberMe\": false}}",
                    Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync($"{Endpoint}/login", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_Admin_User_is_OK()
        {
            StringContent content =
                new(
                    $"{{\"email\": \"{UserSeeds.AdminLogin.Email}\",\"password\": \"{UserSeeds.AdminLogin.Password}\",\"rememberMe\": false}}",
                    Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync($"{Endpoint}/login", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_is_Unauthorized()
        {
            StringContent content =
                new($"{{\"email\": \"{UserSeeds.BasicUser.Email}\",\"password\": \"Basic1234\",\"rememberMe\": false}}",
                    Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync($"{Endpoint}/login", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Is_Logout_OK()
        {
            HttpResponseMessage response = await Client.PostAsync($"{Endpoint}/logout", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Is_Logged_In_OK()
        {
            HttpResponseMessage response1 = await Client.PostAsync($"{Endpoint}/logout", null);
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/IsLoggedIn");

            // TODO - should be unauthorized
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
