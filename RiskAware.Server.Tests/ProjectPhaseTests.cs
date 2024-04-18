using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class ProjectPhaseTests : ServerTestsBase
    {
        public ProjectPhaseTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api/ProjectPhase";
        private const int ProjectId = 1;

        [Fact]
        public async Task GET_Project_Phases_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/{ProjectId}");

            response.EnsureSuccessStatusCode();
            ProjectPhaseDetailDto dto = (await response.Content.ReadFromJsonAsync<ProjectPhaseDetailDto>())!;

            Assert.Equal(ProjectId, dto.Id);
        }
    }
}
