namespace RiskAware.Server.Tests
{
    public class AccountTests(ApiWebApplicationFactory? fixture) : ServerTestsBase(fixture)
    {
        private const string Endpoint = "/api/Account";

        [Fact]
        public async Task Login_is_OK()
        {
            var response = await Client.GetAsync($"{Endpoint}/login");
        }
    }
}
