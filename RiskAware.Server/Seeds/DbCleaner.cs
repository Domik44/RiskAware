using Microsoft.AspNetCore.Identity;
using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public static class DbCleaner
    {
        // todo drop all tables or whole db
        public static async Task ClearAllData(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //var vehicles = context.Vehicles.ToList(); // TODO SMAZAT
            //context.Vehicles.RemoveRange(vehicles); // TODO SMAZAT

            // TODO -> predelat pak do finalni verze -> ted je to urceno pro pokusy
            //var roles = roleManager.Roles.ToList();
            //foreach (var role in roles)
            //{
            //    await roleManager.DeleteAsync(role);
            //}

            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                await userManager.DeleteAsync(user);
            }
            // var user = userManager.Users.First(p => p.FirstName == "Pepa");
            // await userManager.DeleteAsync(user);

            var systemRoles = context.SystemRoles.ToList();
            context.SystemRoles.RemoveRange(systemRoles);

            // var projectPhases = context.ProjectPhases.ToList();
            // context.ProjectPhases.RemoveRange(projectPhases);

            // var comments = context.Comments.ToList();
            // context.Comments.RemoveRange(comments);
            //
            await context.SaveChangesAsync();
        }
    }
}
