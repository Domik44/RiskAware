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
    [Collection("API tests")]
    public class ProjectPhaseTests : ServerTestsBase
    {
        public ProjectPhaseTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api/ProjectPhase";
        private const int ProjectId = 1;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GET_RiskProject_Project_Phases_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);
            HttpResponseMessage response = await Client.GetAsync($"/api/RiskProject/{projectId}/Phases");

            response.EnsureSuccessStatusCode();
            List<ProjectPhaseDto>? dto = (await response.Content.ReadFromJsonAsync<List<ProjectPhaseDto>>())!;

            if (projectId == 1)
            {
                Assert.Equal("Úvodní studie, analýza a specifikace požadavků", dto.First(p => p.Id == 1).Name);
            }
            else
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GET_RiskProject_Project_Phases_is_Unauthorized(int projectId)
        {
            HttpResponseMessage response = await Client.GetAsync($"/api/RiskProject/{projectId}/Phases");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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
        public async Task POST_Project_Phase_is_Unauthorized()
        {
            DtParamsDto dto = new()
            {
                Start = 0, Size = 1, Filters = new List<ColumnFilter>(), Sorting = new List<Sorting>()
            };

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"api/RiskProject/{ProjectId}/Phases", dto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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
                await Client.PostAsJsonAsync($"/api/RiskProject/CreateProjectPhase", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Project_Phase_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage oldPhase = await Client.GetAsync($"api/RiskProject/{ProjectId}/Phases");

            List<ProjectPhaseDto> oldDto = (await oldPhase.Content.ReadFromJsonAsync<List<ProjectPhaseDto>>())!;
            ProjectPhaseDto tmp = oldDto.First(p => p.Id == ProjectId);

            ProjectPhaseCreateDto dto = new()
            {
                Name = tmp.Name,
                Start = tmp.Start,
                End = tmp.End,
                UserRoleType = RoleType.ProjectManager,
                RiskProjectId = ProjectId
            };

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/{ProjectId}", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Project_Phase_is_Unauthorized()
        {
            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/{ProjectId}", new { });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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
