using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskSeed
    {
        public static void Seed(AppDbContext context)
        {
            var risksToBeAdded = new Risk[]
            {
                new()
                {
                    Created = DateTime.Parse("2024-05-20 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 1,
                    ProjectPhaseId = 1,
                    RiskCategoryId = 1
                },
                new()
                {
                    Created = DateTime.Parse("2024-05-21 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 1,
                    ProjectPhaseId = 1,
                    RiskCategoryId = 1
                },
                new()
                {
                    Created = DateTime.Parse("2024-05-24 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 2,
                    RiskCategoryId = 2
                },
                new()
                {
                    Created = DateTime.Parse("2024-05-25 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 2,
                    RiskCategoryId = 2
                },
                new()
                {
                    Created = DateTime.Parse("2024-05-26 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 2,
                    RiskCategoryId = 3
                }
            };

            foreach (var risk in risksToBeAdded)
            {
                if (!context.Risks.Any(p => p.Id == risk.Id))
                {
                    context.Risks.Add(risk);
                }
            }
            context.SaveChanges();
        }
    }
}
