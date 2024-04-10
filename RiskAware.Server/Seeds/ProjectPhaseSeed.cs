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
                    Start = DateTime.Parse("20/05/2024"),
                    End = DateTime.Parse("24/05/2024"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 2,
                    Name = "Návrh a jeho oponentura",
                    Start = DateTime.Parse("24/05/2024"),
                    End = DateTime.Parse("30/05/2024"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 3,
                    Name = "Plán projektu, plán etapy",
                    Start = DateTime.Parse("30/05/2024"),
                    End = DateTime.Parse("03/06/2024"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 4,
                    Name = "Konfigurační řízení, prezentace projektu",
                    Start = DateTime.Parse("03/06/2024"),
                    End = DateTime.Parse("05/06/2024"),
                    RiskProjectId = 1
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
