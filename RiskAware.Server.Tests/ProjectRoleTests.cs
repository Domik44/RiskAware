﻿using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    [Collection("API tests")]
    public class ProjectRoleTests : ServerTestsBase
    {
        public ProjectRoleTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api";
        private const int ProjectId = 1;

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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GET_Project_Roles_is_Unauthorized(int projectId)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Members");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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

            // TODO REDO -> login as user that has project manager role so u can add him to risk project
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{ProjectId}/AddUserToRiskProject", dto);

            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            Assert.True(response.IsSuccessStatusCode);
        }

        // TODO REDO -> DELETE -> methods were deleted
        //[Fact]
        //public async Task POST_Join_Request()
        //{
        //    await PerformLogin(UserSeeds.BasicLogin);

        //    int notAssigned = 2;

        //    HttpResponseMessage response =
        //        await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{notAssigned}/JoinRequest", new { });

        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        // TODO REDO -> DELETE
        // TODO What should the latter entry be?
        //[Fact]
        //public async Task PUT_Approve_Join_Request()
        //{
        //    await PerformLogin(UserSeeds.AdminLogin);

        //    HttpResponseMessage response =
        //        await Client.PutAsJsonAsync(
        //            $"{Endpoint}/RiskProject/{ProjectId}/ApproveJoinRequest/{RoleType.TeamMember}", new { });

        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}


        // TODO What should the latter entry be?
        [Fact]
        public async Task PUT_Approve_Join_Request_is_Unauthorized()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync(
                    $"{Endpoint}/RiskProject/{ProjectId}/ApproveJoinRequest/{RoleType.TeamMember}", new { });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Project_Role_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            // TODO REDO -> must pass id of project role
            // take care that only user that does not have any created risks can be deleted
            //HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/ProjectRole/{ProjectId}");
            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/ProjectRole/2");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // TODO REDO -> DELETE
        // TODO What should the latter entry be?
        //[Fact]
        //public async Task DELETE_Decline_Project_Join_Request_is_OK()
        //{
        //    await PerformLogin(UserSeeds.BasicLogin);

        //    HttpResponseMessage response =
        //        await Client.DeleteAsync(
        //            $"{Endpoint}/RiskProject/{ProjectId}/DeclineJoinRequest/{RoleType.TeamMember}");

        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
