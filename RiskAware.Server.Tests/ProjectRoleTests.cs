using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class ProjectRoleTests : ServerTestsBase
    {
        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        public ProjectRoleTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GET_Project_Roles_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Members");

            response.EnsureSuccessStatusCode();

            List<ProjectRoleDto> dto = (await response.Content.ReadFromJsonAsync<List<ProjectRoleDto>>())!;

            Assert.True(dto.Exists(p => p.RoleName == "ProjectManager"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GET_Project_Roles_is_Unauthorized(int projectId)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Members");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task POST_Add_User_to_Risk_is_OK()
        {
            UserDetailDto userDto = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "František",
                LastName = "Vomáčka",
                Email = "vomacka@test.cz"
            };

            ProjectRoleCreateDto dto = new()
            {
                Name = Guid.NewGuid().ToString(),
                RoleType = RoleType.CommonUser,
                Email = userDto.Email,
                UserRoleType = RoleType.ProjectManager,
                ProjectPhaseId = 1
            };

            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage responseUserCreate =
                await Client.PostAsJsonAsync($"{Endpoint}/User", userDto);
            responseUserCreate.EnsureSuccessStatusCode();

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/AddUserToRiskProject", dto);

            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task POST_Add_User_to_Risk_is_Unauthorized()
        {
            UserDetailDto userDto = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "František",
                LastName = "Vomáčka",
                Email = "vomacka@test.cz"
            };

            ProjectRoleCreateDto dto = new()
            {
                Name = Guid.NewGuid().ToString(),
                RoleType = RoleType.CommonUser,
                Email = userDto.Email,
                UserRoleType = RoleType.ProjectManager,
                ProjectPhaseId = 1
            };

            await PerformLogin(UserSeeds.BasicLogin);
            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/AddUserToRiskProject", dto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task POST_Join_Request_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int notAssigned = 2;

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{notAssigned}/JoinRequest", new { });

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_Join_Request_is_Unauthorized()
        {
            int notAssigned = 2;

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{notAssigned}/JoinRequest", new { });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // TODO What should the latter entry be?
        [Fact]
        public async Task PUT_Approve_Join_Request_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync(
                    $"{Endpoint}/RiskProject/{ProjectId}/ApproveJoinRequest/{RoleType.TeamMember}", new { });

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // TODO What should the latter entry be?
        [Fact]
        public async Task PUT_Approve_Join_Request_is_Unauthorized()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync(
                    $"{Endpoint}/RiskProject/{ProjectId}/ApproveJoinRequest/{RoleType.TeamMember}", new { });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Project_Role_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/ProjectRole/{ProjectId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Project_Role_is_Unauthorized()
        {
            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/ProjectRole/{ProjectId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // TODO What should the latter entry be?
        [Fact]
        public async Task DELETE_Decline_Project_Join_Request_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.DeleteAsync(
                    $"{Endpoint}/RiskProject/{ProjectId}/DeclineJoinRequest/{RoleType.TeamMember}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // TODO What should the latter entry be?
        [Fact]
        public async Task DELETE_Decline_Project_Join_Request_is_Unauthorized()
        {
            HttpResponseMessage response =
                await Client.DeleteAsync(
                    $"{Endpoint}/RiskProject/{ProjectId}/DeclineJoinRequest/{RoleType.TeamMember}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
