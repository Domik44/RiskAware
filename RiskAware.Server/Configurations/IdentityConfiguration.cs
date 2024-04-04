using Microsoft.AspNetCore.Identity;

namespace RiskAware.Server.Configurations;

public static class IdentityConfiguration
{
    public static void ConfigureIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    }
}
