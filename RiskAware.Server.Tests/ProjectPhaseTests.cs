using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

        [Fact]
        public async Task GET_RiskProject_Project_Phases_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);
            HttpResponseMessage response = await Client.GetAsync($"/api/RiskProject/{ProjectId}/Phases");

            response.EnsureSuccessStatusCode();
            List<ProjectPhaseDto>? dto = (await response.Content.ReadFromJsonAsync<List<ProjectPhaseDto>>())!;

            Assert.Equal("Úvodní studie, analýza a specifikace požadavků", dto.First(p => p.Id == 1).Name);
        }

        [Fact]
        public async Task POST_Project_Phase_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            DtParamsDto dto = new()
            {
                Start = 0, Size = 1, Filters = new List<ColumnFilter>(), Sorting = new List<Sorting>()
            };

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"api/RiskProject/{ProjectId}/Phases", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_Risk_Project_Create_Phase_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            ProjectPhaseCreateDto dto = new()
            {
                Name = "Test", Start = DateTime.Now, End = DateTime.Now, UserRoleType = RoleType.ProjectManager
            };

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"/api/RiskProject/{ProjectId}/CreateProjectPhase", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Project_Phase_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage oldPhase = await Client.GetAsync($"{Endpoint}/{ProjectId}");
            ProjectPhaseDetailDto oldDto = (await oldPhase.Content.ReadFromJsonAsync<ProjectPhaseDetailDto>())!;

            ProjectPhaseDto dto = new()
            {
                Id = oldDto.Id,
                Order = 0,
                Name = Guid.NewGuid().ToString(),
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(3),
                Risks = []
            };

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/{ProjectId}", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Project_Phase_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/{ProjectId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
