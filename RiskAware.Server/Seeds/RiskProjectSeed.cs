using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskProjectSeed
    {
        public static void Seed(AppDbContext context)
        {
            var projectsToSeed = new List<RiskProject>
            {
                new RiskProject()
                {
                    Title = "MPR projekt",
                    Description = "Velice zajímavý projekt, který Vás mnoho naučí.",
                    Start = DateTime.Parse("2024-02-13"),
                    End = DateTime.Parse("2024-06-01"),
                    IsValid = true,
                    IsBlank = false,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Nenastaveny_Pepa_Brnak",
                    Description = "Popis2",
                    Start = DateTime.Parse("2024-02-16"),
                    End = DateTime.Parse("2024-06-14"),
                    IsValid = true,
                    IsBlank = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Nastaveny_neni_PM",
                    Description = "Popis3",
                    Start = DateTime.Parse("2024-02-27"),
                    End = DateTime.Parse("2024-06-30"),
                    IsValid = true,
                    IsBlank = false,
                    Scale = 3,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Nenastaveny_nema_roli",
                    Description = "Popis4",
                    Start = DateTime.Parse("2024-03-03"),
                    End = DateTime.Parse("2024-07-31"),
                    IsValid = true,
                    IsBlank = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Proj5",
                    Description = "Popis5",
                    Start = DateTime.Parse("2024-03-15"),
                    End = DateTime.Parse("2024-08-31"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
                new RiskProject()
                {
                    Title = "Proj6",
                    Description = "Popis6",
                    Start = DateTime.Parse("2024-04-04"),
                    End = DateTime.Parse("2024-09-20"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                },
            };

            DateTime start = DateTime.Parse("2024-08-01");
            DateTime end = DateTime.Parse("2024-10-30");
            for (int i = 7; i < 55; i++)
            {
                start = start.AddDays(7);
                end = end.AddDays(7);
                var project = new RiskProject()
                {
                    Title = $"Proj{i}",
                    Description = $"Popis{i}",
                    Start = start,
                    End = end,
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                };
                projectsToSeed.Add(project);
            }

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
