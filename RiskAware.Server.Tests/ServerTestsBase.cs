using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Seeds;
using System.Net.Http.Json;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace RiskAware.Server.Tests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
    }

    public abstract class ServerTestsBase : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
    {
        private readonly ApiWebApplicationFactory Factory;
        protected readonly HttpClient Client;
        private SqlConnection? _connection;
        private Respawner? _respawner;

        public ITestOutputHelper TestOutputHelper;

        protected ServerTestsBase(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture)
        {
            TestOutputHelper = testOutputHelper;
            Factory = fixture;
            Client = Factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            string? connectionString =
                Factory.Services.GetRequiredService<IConfiguration>().GetConnectionString("TestConnection");
            _connection = new SqlConnection(connectionString);
            await _connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(_connection,
                new RespawnerOptions
                {
                    SchemasToInclude = new[] {"dbo"}, DbAdapter = DbAdapter.SqlServer, WithReseed = true
                });

            await _respawner.ResetAsync(_connection);

            await DbSeeder.SeedAll(Factory.Services);
        }

        public async Task DisposeAsync()
        {
            Client.Dispose();

            if (_connection != null)
            {
                if (_respawner != null)
                {
                    await _respawner.ResetAsync(_connection);
                }

                await _connection.CloseAsync();
            }
        }

        public async Task PerformLogin(LoginDto loginDto)
        {
            LoginDto user = new() {Email = loginDto.Email, Password = loginDto.Password};

            HttpResponseMessage res = await Client.PostAsJsonAsync("api/Account/login", user);

            if (res.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? cookies))
            {
                string authCookie = cookies.First();
                authCookie = authCookie.Replace("auth_cookie=", string.Empty);
            }
        }
    }
}
