using NuGet.Protocol;
using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.DatatableDTOs;
using RiskAware.Server.DTOs.RiskProjectDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    [Collection("API tests")]
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

        [Fact]
        public async Task GET_Admin_Risk_Projects_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/AdminRiskProjects");
            response.EnsureSuccessStatusCode();

            List<RiskProjectDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskProjectDto>>())!;

            TestOutputHelper.WriteLine(dto.Count.ToString());

            Assert.True(dto.Exists(p =>
                p.ProjectManagerName == $"{UserSeeds.BasicUser.FirstName} {UserSeeds.BasicUser.LastName}"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GET_Risk_Project_by_Id_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}");
            response.EnsureSuccessStatusCode();

            RiskProjectPageDto dto = (await response.Content.ReadFromJsonAsync<RiskProjectPageDto>())!;

            Assert.Equal(projectId, dto.Detail.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GET_Risk_Project_Detail_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Detail");
            response.EnsureSuccessStatusCode();

            RiskProjectDetailDto dto = (await response.Content.ReadFromJsonAsync<RiskProjectDetailDto>())!;

            if (projectId == 1)
            {
                Assert.Equal("MPR projekt", dto.Title);
            }
            else
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GET_Risk_Project_Comments_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{ProjectId}/GetComments");
            response.EnsureSuccessStatusCode();

            List<CommentDto> dto = (await response.Content.ReadFromJsonAsync<List<CommentDto>>())!;

            Assert.True(dto.Exists(c => c.Text == "Tenhle projekt je Bomba!!!"));
        }

        [Fact]
        public async Task POST_Risk_Projects_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            DtParamsDto paramsDto = new()
            {
                Start = 0, Size = 1, Filters = new List<ColumnFilter>(), Sorting = new List<Sorting>()
            };

            HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/RiskProjects", paramsDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_Risk_Projects_User_Risk_Projects_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            DtParamsDto paramsDto = new()
            {
                Start = 0, Size = 1, Filters = new List<ColumnFilter>(), Sorting = new List<Sorting>()
            };

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/UserRiskProjects", paramsDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_Risk_Project_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            RiskProjectCreateDto dto = new()
            {
                Title = "Test project",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                Email = UserSeeds.BasicUser.Email
            };

            HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/RiskProject", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_Risk_Project_Add_Comment_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            int projectId = 1;
            string text = "foo";
            string query = $"riskProjectId={projectId}&text={text}";

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/AddComment?{query}", new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Risk_Project_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage dto = await Client.GetAsync($"{Endpoint}/RiskProject/{ProjectId}/Detail");
            dto.EnsureSuccessStatusCode();

            RiskProjectDetailDto newDto = (await dto.Content.ReadFromJsonAsync<RiskProjectDetailDto>())!;
            newDto.Description = "Test description";

            StringContent content = new(newDto.ToJson(), System.Text.Encoding.UTF8, "application/json");
            TestOutputHelper.WriteLine(content.ReadAsStringAsync().Result);

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}", newDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Risk_Project_Initial_Project_Setup_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            RiskProjectCreateDto createDto = new()
            {
                Title = "Test project",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
                Email = UserSeeds.BasicUser.Email
            };

            HttpResponseMessage response1 = await Client.PostAsJsonAsync($"{Endpoint}/RiskProject", createDto);
            response1.EnsureSuccessStatusCode();

            HttpResponseMessage projectsResponse = await Client.GetAsync($"{Endpoint}/RiskProjects");
            projectsResponse.EnsureSuccessStatusCode();

            List<RiskProjectDto> projects = (await projectsResponse.Content.ReadFromJsonAsync<List<RiskProjectDto>>())!;

            HttpResponseMessage projectDetail =
                await Client.GetAsync(
                    $"{Endpoint}/RiskProject/{projects.First(p => p.Title == createDto.Title).Id}/Detail");
            projectDetail.EnsureSuccessStatusCode();

            RiskProjectDetailDto projectDetailDto =
                (await projectDetail.Content.ReadFromJsonAsync<RiskProjectDetailDto>())!;

            projectDetailDto.Title = "New title";


            HttpResponseMessage response =
                await Client.PutAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/InitialProjectSetup",
                    projectDetailDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Risk_Project_Restore_Project()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/RestoreProject", new { });

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                HttpResponseMessage resp = await Client.DeleteAsync($"{Endpoint}/RiskProject/{ProjectId}");
                resp.EnsureSuccessStatusCode();
                response = await Client.PutAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/RestoreProject", new { });
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Risk_Project_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/RiskProject/{ProjectId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                HttpResponseMessage rep =
                    await Client.PutAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/RestoreProject", new { });
                rep.EnsureSuccessStatusCode();
                response = await Client.DeleteAsync($"{Endpoint}/RiskProject/{ProjectId}");
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
