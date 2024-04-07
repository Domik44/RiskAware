using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class ProjectPhaseSeed
    {
        public static void Seed(AppDbContext context)
        {
            var projectPhasesToBeAdded = new ProjectPhase[]
            {
                new()
                {
                    Id = Guid.Parse("e9523584-4956-4067-9079-90abdf8574ed"),
                    Order = 1,
                    Name = "Úvodní studie, analýza a specifikace požadavků",
                    Start = DateTime.Parse("20/05/2024"),
                    End = DateTime.Parse("24/05/2024"),
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea")
                },
                new()
                {
                    Id = Guid.Parse("9d1f6672-3199-4960-8d15-a8f832f971f0"),
                    Order = 2,
                    Name = "Návrh a jeho oponentura",
                    Start = DateTime.Parse("24/05/2024"),
                    End = DateTime.Parse("30/05/2024"),
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea")
                },
                new()
                {
                    Id = Guid.Parse("1f6bc20d-c318-479b-8983-0838af882e15"),
                    Order = 3,
                    Name = "Plán projektu, plán etapy",
                    Start = DateTime.Parse("30/05/2024"),
                    End = DateTime.Parse("03/06/2024"),
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea")
                },
                new()
                {
                    Id = Guid.Parse("5244028a-76b4-4047-8005-c21882a6ee5e"),
                    Order = 4,
                    Name = "Konfigurační řízení, prezentace projektu",
                    Start = DateTime.Parse("03/06/2024"),
                    End = DateTime.Parse("05/06/2024"),
                    RiskProjectId = Guid.Parse("6a45e6b5-f5db-458e-a26e-4d5ad85fbcea")
                }
            };

            foreach (var phase in projectPhasesToBeAdded)
            {
                if(!context.ProjectPhases.Any(p => p.Id == phase.Id))
                {
                    context.ProjectPhases.Add(phase);
                }
            }
            context.SaveChanges();
        }
    }
}
