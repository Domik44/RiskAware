using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    [Collection("API tests")]
    public class ProjectRoleTests : ServerTestsBase
    {
        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        public ProjectRoleTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        // TODO REDO -> change to POST and see filtering
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public async Task GET_Project_Roles_is_OK(int projectId)
        //{
        //    await PerformLogin(UserSeeds.BasicLogin);

        //    HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Members");

        //    response.EnsureSuccessStatusCode();

        //    List<ProjectRoleDto> dto = (await response.Content.ReadFromJsonAsync<List<ProjectRoleDto>>())!;

        //    Assert.True(dto.Exists(p => p.RoleName == "ProjectManager"));
        //}

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

            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/AddUserToRiskProject", dto);

            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
