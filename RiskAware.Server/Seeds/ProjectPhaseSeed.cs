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
                    Order = 1,
                    Name = "Úvodní studie, analýza a specifikace požadavků",
                    Start = DateTime.Parse("2024-05-20"),
                    End = DateTime.Parse("2024-05-24"),
                    RiskProjectId = 1,
                    ProjectRoleId = 3
                },
                new()
                {
                    Order = 2,
                    Name = "Návrh a jeho oponentura",
                    Start = DateTime.Parse("2024-05-24"),
                    End = DateTime.Parse("2024-05-30"),
                    RiskProjectId = 1,
                    ProjectRoleId = 4
                },
                new()
                {
                    Order = 3,
                    Name = "Plán projektu, plán etapy",
                    Start = DateTime.Parse("2024-05-30"),
                    End = DateTime.Parse("2024-06-03"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 4,
                    Name = "Konfigurační řízení, prezentace projektu",
                    Start = DateTime.Parse("2024-06-03"),
                    End = DateTime.Parse("2024-06-05"),
                    RiskProjectId = 1
                }
            };

            foreach (var phase in projectPhasesToBeAdded)
            {
                if (!context.ProjectPhases.Any(p => p.Id == phase.Id))
                {
                    context.ProjectPhases.Add(phase);
                }
            }
            context.SaveChanges();
        }
    }
}
