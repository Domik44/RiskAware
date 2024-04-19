﻿using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class UserTests : ServerTestsBase
    {
        public UserTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        [Fact]
        public async Task GET_User_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/User");

            response.EnsureSuccessStatusCode();
            List<UserDto> dto = (await response.Content.ReadFromJsonAsync<List<UserDto>>())!;

            Assert.True(dto.Exists(p =>
                p.FullName == $"{UserSeeds.BasicUser.FirstName} {UserSeeds.BasicUser.LastName}"));
        }

        [Fact]
        public async Task GET_User_by_Id_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/User/{UserSeeds.BasicUser.Id}");
            response.EnsureSuccessStatusCode();

            TestOutputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);

            UserDetailDto dto = (await response.Content.ReadFromJsonAsync<UserDetailDto>())!;

            Assert.Equal($"{UserSeeds.BasicUser.FirstName} {UserSeeds.BasicUser.LastName}",
                $"{dto.FirstName} {dto.LastName}");
        }

        [Fact]
        public async Task POST_User_is_OK()
        {
            UserDetailDto userDto = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "František",
                LastName = "Vomáčka",
                Email = "vomacka@seznam.cz"
            };

            // TODO not authorized with admin login
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response = await Client.PostAsJsonAsync($"{Endpoint}/User", userDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_User_is_OK()
        {
            UserDetailDto userDto = new()
            {
                Id = UserSeeds.BasicUser.Id,
                FirstName = "František",
                LastName = "Vomáčka",
                Email = "vomacka@seznam.cz"
            };

            UserSeeds.BasicUser = new User
            {
                Id = userDto.Id,
                UserName = userDto.Email,
                Email = userDto.Email,
                EmailConfirmed = true,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                SystemRole = UserSeeds.BasicUser.SystemRole
            };

            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/User/{userDto.Id}", userDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_User_Change_Password_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync($"{Endpoint}/User/{UserSeeds.BasicUser.Id}/ChangePassword", new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_User_Restore_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response =
                await Client.PutAsJsonAsync($"{Endpoint}/User/{UserSeeds.BasicUser.Id}/Restore", new { });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_User_is_OK()
        {
            await PerformLogin(UserSeeds.AdminLogin);

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/User/{UserSeeds.BasicUser.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
