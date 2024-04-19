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
        private readonly ApiWebApplicationFactory Factory;
        protected readonly HttpClient Client;

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
    }
}
