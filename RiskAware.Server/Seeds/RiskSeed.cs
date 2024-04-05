using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskSeed
    {
        public static void Seed(AppDbContext context)
        {
            var RisksToBeAdded = new Risk[]
            {
                new()
                {
                    Id = Guid.Parse("5105eac4-d3e3-46d5-bc96-ea935d33860f"),
                    Created = DateTime.Parse("20/05/2024 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    ProjectPhaseId = Guid.Parse("e9523584-4956-4067-9079-90abdf8574ed"),
                },
                new()
                {
                    Id = Guid.Parse("dffb5b4f-5ddd-4c0b-91e0-4d42ab4a2e94"),
                    Created = DateTime.Parse("21/05/2024 12:00:00"),
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    ProjectPhaseId = Guid.Parse("e9523584-4956-4067-9079-90abdf8574ed"),
                },
                new()
                {
                    Id = Guid.Parse("9aaad200-bab8-46b3-b85c-7ffb457adf5c"),
                    Created = DateTime.Parse("24/05/2024 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    ProjectPhaseId = Guid.Parse("9d1f6672-3199-4960-8d15-a8f832f971f0"),
                },
                new()
                {
                    Id = Guid.Parse("3cc518b6-0f1a-4def-988a-de407715a12e"),
                    Created = DateTime.Parse("25/05/2024 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    ProjectPhaseId = Guid.Parse("9d1f6672-3199-4960-8d15-a8f832f971f0"),
                },
                new()
                {
                    Id = Guid.Parse("285ce42a-41d6-47da-9334-38a21dea6728"),
                    Created = DateTime.Parse("26/05/2024 12:00:00"),
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea"),
                    ProjectPhaseId = Guid.Parse("9d1f6672-3199-4960-8d15-a8f832f971f0"),
                }
            };

            foreach(var risk in RisksToBeAdded)
            {
                if(!context.Risks.Any(p =>  p.Id == risk.Id))
                {
                    context.Risks.Add(risk);
                }
            }
            context.SaveChanges();
        }
    }
}
