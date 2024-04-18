using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Tests.Seeds
{
    public static class UserSeeds
    {
        public static SystemRole AdminRole = new()
        {
            Name = "Admin", IsAdministrator = true, Description = "System admin"
        };

        public static readonly SystemRole BasicRole = new()
        {
            Name = "Basic", IsAdministrator = false, Description = "Basic app user"
        };

        public static User AdminUser = new()
        {
            Id = "d6f46418-2c21-43f8-b167-162fb5e3a999",
            UserName = "admin@google.com",
            Email = "admin@google.com",
            EmailConfirmed = true,
            FirstName = "HonzaAdmin",
            LastName = "Zvesnice",
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
    }
}
