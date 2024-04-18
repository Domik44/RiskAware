using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Respawn;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;
using RiskAware.Server.Tests.Seeds;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;

namespace RiskAware.Server.Tests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
    }

    public abstract class ServerTestsBase : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
    {
        protected readonly ApiWebApplicationFactory Factory;
        protected HttpClient Client;

        private SqlConnection? _connection;
        private Respawner? _respawner;

        public ITestOutputHelper TestOutputHelper;

        protected ServerTestsBase(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory? fixture)
        {
            TestOutputHelper = testOutputHelper;
            Factory = fixture;
            Client = Factory.CreateClient(
                new WebApplicationFactoryClientOptions {AllowAutoRedirect = true});
        }

        public async Task InitializeAsync()
        {
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


        private async Task SeedUsersData()
        {
            IServiceScopeFactory? scopeFactory = Factory.Services.GetService<IServiceScopeFactory>();
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                AppDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (applicationDbContext == null)
                {
                    throw new Exception("Cannot get Application context to seed users data");
                }

                EntityEntry<User> admin = applicationDbContext.Users.Add(UserSeeds.AdminUser);

                EntityEntry<User> basic = applicationDbContext.Users.Add(UserSeeds.BasicUser);
                await applicationDbContext.SaveChangesAsync();
            }
        }

        // protected async Task<Case> SeedCase(string name, string description, UserId ownerId,
        //     List<UserId>? analysts = null, List<UserId>? viewers = null)
        // {
        //     analysts ??= new List<UserId>();
        //     viewers ??= new List<UserId>();
        //
        //     RequestsAsUser(ownerId);
        //
        //     HttpResponseMessage response = await Client.PostAsJsonAsync("/cases", new CaseRequest(name, description));
        //
        //     if (response.StatusCode != HttpStatusCode.OK)
        //     {
        //         throw new Exception("Cannot seed case");
        //     }
        //
        //     var caseResponse = await response.Content.ReadFromJsonAsync<CaseResponse>();
        //     if (caseResponse == null)
        //     {
        //         throw new Exception("Cannot seed case");
        //     }
        //
        //     foreach (var analystId in analysts)
        //     {
        //         var analyst = new AddAnalystRequest(analystId);
        //         HttpResponseMessage analystResponse =
        //             await Client.PostAsJsonAsync($"/cases/{caseResponse.Id.ToString()}/analysts",
        //                 analyst);
        //         if (analystResponse.StatusCode != HttpStatusCode.OK)
        //         {
        //             throw new Exception(
        //                 $"User {analystId.Value.ToString()} cannot be added as an analyst to case {caseResponse.Id}");
        //         }
        //     }
        //
        //     foreach (var viewerId in viewers)
        //     {
        //         var analyst = new AddViewerRequest(viewerId);
        //         HttpResponseMessage analystResponse =
        //             await Client.PostAsJsonAsync($"/cases/{caseResponse.Id.ToString()}/viewers",
        //                 analyst);
        //         if (analystResponse.StatusCode != HttpStatusCode.OK)
        //         {
        //             throw new Exception(
        //                 $"User {viewerId.Value.ToString()} cannot be added as a viewer to case {caseResponse.Id}");
        //         }
        //     }
        //
        //     return caseResponse;
        // }
    }
}
