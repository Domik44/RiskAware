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
        public RiskCategoryTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        [Fact]
        public async Task GET_Risk_Categories_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{ProjectId}/RiskCategories");

            response.EnsureSuccessStatusCode();

            List<RiskCategoryDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskCategoryDto>>())!;

            Assert.True(dto.Exists(p => p.Name == "Finanční rizika"));
        }

        [Fact]
        public async Task GET_Risk_Category_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int categoryId = 1;

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskCategory/{categoryId}");

            response.EnsureSuccessStatusCode();

            RiskCategoryDto dto = (await response.Content.ReadFromJsonAsync<RiskCategoryDto>())!;

            Assert.Equal("Finanční rizika", dto.Name);
        }

        [Fact]
        public async Task POST_Risk_Category_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            RiskCategoryDto dto = new() {Id = 0, Name = "Testovací rizika"};
            string query = $"riskId={ProjectId}";

            HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/RiskCategory?{query}", dto);

            response.EnsureSuccessStatusCode();

            // RiskCategoryDto createdDto = (await response.Content.ReadFromJsonAsync<RiskCategoryDto>())!;
            // Assert.Equal("Testovací rizika", createdDto.Name);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Risk_Category_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            RiskCategory dto = new() {Name = "Testovací kategorie"};
            int categoryId = 1;

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/RiskCategory/{categoryId}", dto);

            response.EnsureSuccessStatusCode();

            // RiskCategoryDto updatedDto = (await response.Content.ReadFromJsonAsync<RiskCategory>())!;
            // Assert.Equal("Testovací kategorie", updatedDto.Name);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Risk_Category_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int categoryId = 1;

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/RiskCategory/{categoryId}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
