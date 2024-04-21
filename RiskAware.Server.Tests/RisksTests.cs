using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class RisksTests : ServerTestsBase
    {
        private const string Endpoint = "/api";
        private const int ProjectId = 1;

        public RisksTests(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture) : base(
            testOutputHelper, fixture)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GET_Risks_is_OK(int id)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/Risk/{id}");
            response.EnsureSuccessStatusCode();

            RiskDetailDto dto = (await response.Content.ReadFromJsonAsync<RiskDetailDto>())!;

            Assert.Equal($"Riziko {id}", dto.Title);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GET_Risks_is_Unauthorized(int id)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/Risk/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GET_Risk_Project_Risks_is_OK(int projectId)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Risks");

            response.EnsureSuccessStatusCode();
            List<RiskDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskDto>>())!;

            if (projectId == 1)
            {
                Assert.True(dto.Exists(r => r.Title == $"Riziko {projectId}"));
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
        public async Task GET_Risk_Project_Risks_is_Unauthorized(int projectId)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/RiskProject/{projectId}/Risks");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GET_Project_Phase_Risks_is_OK(int id)
        {
            await PerformLogin(UserSeeds.BasicLogin);

            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/ProjectPhase/{id}/Risks");
            response.EnsureSuccessStatusCode();

            List<RiskDto> dto = (await response.Content.ReadFromJsonAsync<List<RiskDto>>())!;

            Assert.True(dto.Exists(r => r.Title == $"Riziko {id + 1}"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GET_Project_Phase_Risks_is_Unauthorized(int id)
        {
            HttpResponseMessage response = await Client.GetAsync($"{Endpoint}/ProjectPhase/{id}/Risks");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task POST_Add_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            // TODO REDO -> wrong DTO format
            RiskCreateDto dto = new()
            {
                Title = "Testovací riziko", Probability = 1, Impact = 1, Threat = "Testovací riziko"
            };

            HttpResponseMessage response =
                await Client.PostAsJsonAsync($"{Endpoint}/RiskProject/{1}/AddRisk", dto);

            TestOutputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Update_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            // TODO REDO -> wrong DTO format
            RiskCreateDto dto = new()
            {
                Title = "Testovací riziko", Probability = 1, Impact = 1, Threat = "Testovací riziko"
            };

            int id = 1;

            HttpResponseMessage response = await Client.PutAsJsonAsync($"{Endpoint}/Risk/{id}", dto);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Restore_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int id = 1;

            HttpResponseMessage response = await Client.PutAsync($"{Endpoint}/Risk/{id}/Restore", null);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Approve_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int id = 1;

            HttpResponseMessage response = await Client.PutAsync($"{Endpoint}/Risk/{id}/Approve", null);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Reject_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int id = 1;

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/Risk/{id}/Reject");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DELETE_Risk_is_OK()
        {
            await PerformLogin(UserSeeds.BasicLogin);

            int id = 1;

            HttpResponseMessage response = await Client.DeleteAsync($"{Endpoint}/Risk/{id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
