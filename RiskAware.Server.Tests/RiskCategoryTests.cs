using RiskAware.Server.DTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class RiskCategoryTests : ServerTestsBase
    {
        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        public RiskCategoryTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GET_Risk_Categories_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/RiskCategories");

            response.EnsureSuccessStatusCode();

            List<RiskCategoryDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskCategoryDto>>())!;

            Assert.True(dto.Exists(p => p.Name == "Finanční rizika"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GET_Risk_Categories_is_Unauthorized(int projectId)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/RiskCategories");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_Risk_Category_is_OK(int categoryId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskCategory/{categoryId}");

            response.EnsureSuccessStatusCode();

            RiskCategoryDto dto = (await response.Content.ReadFromJsonAsync<RiskCategoryDto>())!;

            Assert.Equal("Finanční rizika", dto.Name);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_Risk_Category_is_Unauthorized(int categoryId)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskCategory/{categoryId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task POST_Risk_Category_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            RiskCategoryDto dto = new() {Id = 0, Name = "Testovací rizika"};
            string query = $"riskId={ProjectId}";

            HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/RiskCategory/{ProjectId}", dto); // Was using wrong endpoint
            //HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/RiskCategory?{query}", dto);

            response.EnsureSuccessStatusCode();

            // RiskCategoryDto createdDto = (await response.Content.ReadFromJsonAsync<RiskCategoryDto>())!;
            // Assert.Equal("Testovací rizika", createdDto.Name);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
