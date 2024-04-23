using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Seeds;
using System.Data.Common;
using System.Net.Http.Json;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace RiskAware.Server.Tests
{
    public class ApiWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AppDbContext>));

                services.Remove(dbContextDescriptor);

                ServiceDescriptor? dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    SqliteConnection connection = new("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    DbConnection connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
            });

            builder.UseEnvironment("Development");
        }
    }

    public abstract class ServerTestsBase : IClassFixture<ApiWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly ApiWebApplicationFactory<Program> Factory;
        protected readonly HttpClient Client;

        public ITestOutputHelper TestOutputHelper;

        protected ServerTestsBase(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Program>? fixture)
        {
            TestOutputHelper = testOutputHelper;
            Factory = fixture;
            Client = Factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            using IServiceScope scope = Factory.Services.CreateScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            await DbSeeder.SeedAll(Factory.Services);
        }

        public async Task DisposeAsync()
        {
            Client.Dispose();
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
