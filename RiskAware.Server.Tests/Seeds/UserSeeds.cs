using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.DTOs.UserDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.Tests.Seeds
{
    public static class UserSeeds
    {
        private static readonly SystemRole AdminRole = new()
        {
            Name = "Admin", IsAdministrator = true, Description = "System admin"
        };

        private static readonly SystemRole BasicRole = new()
        {
            Name = "Basic", IsAdministrator = false, Description = "Basic app user"
        };

        public static User AdminUser = new()
        {
            Id = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
            UserName = "zb@google.com",
            Email = "zb@google.com",
            EmailConfirmed = true,
            FirstName = "Zdenda",
            LastName = "Branik",
            SystemRole = AdminRole
        };

        public static User BasicUser = new()
        {
            Id = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
            UserName = "pb@google.com",
            Email = "pb@google.com",
            EmailConfirmed = true,
            FirstName = "Pepa",
            LastName = "Brnak",
            SystemRole = BasicRole
        };

        public static LoginDto AdminLogin = new() {Email = AdminUser.Email, Password = "Basic123"};

        public static LoginDto BasicLogin = new() {Email = BasicUser.Email, Password = "Basic123"};
    }
}
