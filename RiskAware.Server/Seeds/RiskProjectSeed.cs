using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskProjectSeed
    {
        public static void Seed(AppDbContext context)
        {
            var projectsToSeed = new RiskProject[]
            {
                new RiskProject()
                {
                    Title = "PokusProj",
                    Description = "Popis tohoto projektu muze byt velice zajimavy, ale ja nejsem kreativni :)",
                    Start = DateTime.Parse("2024-02-13"),
                    End = DateTime.Parse("2024-06-01"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Proj2",
                    Description = "Popis2",
                    Start = DateTime.Parse("2024-02-16"),
                    End = DateTime.Parse("2024-06-14"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Proj3",
                    Description = "Popis3",
                    Start = DateTime.Parse("2024-02-27"),
                    End = DateTime.Parse("2024-06-30"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Proj4",
                    Description = "Popis4",
                    Start = DateTime.Parse("2024-03-03"),
                    End = DateTime.Parse("2024-07-31"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2"
                },
                new RiskProject()
                {
                    Title = "Proj5",
                    Description = "Popis5",
                    Start = DateTime.Parse("2024-03-15"),
                    End = DateTime.Parse("2024-08-31"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2"
                },
                new RiskProject()
                {
                    Title = "Proj6",
                    Description = "Popis6",
                    Start = DateTime.Parse("2024-04-04"),
                    End = DateTime.Parse("2024-09-20"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8"
                },
            };

            foreach (var project in projectsToSeed)
            {
                if (!context.RiskProjects.Any(p => p.Id == project.Id))
                {
                    context.RiskProjects.Add(project);
                }
            }

            context.SaveChanges();
        }
    }
}
