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
                    Start = DateTime.Now,
                    End = DateTime.Parse("24/08/2024"),
                    IsValid = true,
                    Scale = 5,
                    UserId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
                }
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
