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
                    Created = DateTime.Parse("2024-04-05 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 1,
                    ProjectPhaseId = 1,
                    RiskCategoryId = 1
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-06 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = 1,
                    ProjectPhaseId = 1,
                    RiskCategoryId = 2
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-09 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 2,
                    RiskCategoryId = 2
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-09 16:42:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 2,
                    RiskCategoryId = 3
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-12 18:00:00"),
                    UserId = "e81e8eab-2dd2-45ee-8d74-54822c8e69f2",
                    RiskProjectId = 1,
                    ProjectPhaseId = 3,
                    RiskCategoryId = 3
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-14 10:00:00"),
                    UserId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    RiskProjectId = 1,
                    ProjectPhaseId = 4,
                    RiskCategoryId = 4
                },
                new()
                {
                    Created = DateTime.Parse("2024-04-18 09:00:00"),
                    UserId = "12749a7a-100b-4e69-a234-7d059e508d5b",
                    RiskProjectId = 1,
                    ProjectPhaseId = 5,
                    RiskCategoryId = 1
                },
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
