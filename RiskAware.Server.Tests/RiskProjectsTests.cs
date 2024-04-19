using Microsoft.AspNetCore.Http;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Tests.Seeds;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class RiskProjectsTests : ServerTestsBase
    {
        public RiskProjectsTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        [Fact]
        public async Task GET_Risk_Projects_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProjects");

            response.EnsureSuccessStatusCode();

            List<RiskProjectDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskProjectDto>>())!;

            Assert.True(dto.Exists(p =>
                p.ProjectManagerName == $"{UserSeeds.BasicUser.FirstName} {UserSeeds.BasicUser.LastName}"));
        }
    }
}
