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
                    Start = DateTime.Parse("2024-04-05"),
                    End = DateTime.Parse("2024-04-07"),
                    RiskProjectId = 1,
                },
                new()
                {
                    Order = 2,
                    Name = "Návrh a jeho oponentura",
                    Start = DateTime.Parse("2024-04-08"),
                    End = DateTime.Parse("2024-04-10"),
                    RiskProjectId = 1,
                },
                new()
                {
                    Order = 3,
                    Name = "Plán projektu, plán etapy",
                    Start = DateTime.Parse("2024-04-11"),
                    End = DateTime.Parse("2024-04-13"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 4,
                    Name = "Konfigurační řízení, prezentace projektu",
                    Start = DateTime.Parse("2024-04-14"),
                    End = DateTime.Parse("2024-04-15"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 5,
                    Name = "Implementace",
                    Start = DateTime.Parse("2024-04-16"),
                    End = DateTime.Parse("2024-04-22"),
                    RiskProjectId = 1,
                },
                new()
                {
                    Order = 6,
                    Name = "Testování, změnové řízení",
                    Start = DateTime.Parse("2024-04-23"),
                    End = DateTime.Parse("2024-05-26"),
                    RiskProjectId = 1,
                },
                new()
                {
                    Order = 7,
                    Name = "Uzavření projektu",
                    Start = DateTime.Parse("2024-04-27"),
                    End = DateTime.Parse("2024-04-28"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 8,
                    Name = "Produktové metriky",
                    Start = DateTime.Parse("2024-04-29"),
                    End = DateTime.Parse("2024-05-01"),
                    RiskProjectId = 1
                },
                new()
                {
                    Order = 1,
                    Name = "Úvodní studie, analýza a specifikace požadavků",
                    Start = DateTime.Parse("2024-05-20"),
                    End = DateTime.Parse("2024-05-24"),
                    RiskProjectId = 3,
                },
                new()
                {
                    Order = 2,
                    Name = "Návrh a jeho oponentura",
                    Start = DateTime.Parse("2024-05-24"),
                    End = DateTime.Parse("2024-05-30"),
                    RiskProjectId = 3,
                },
                new()
                {
                    Order = 3,
                    Name = "Plán projektu, plán etapy",
                    Start = DateTime.Parse("2024-05-30"),
                    End = DateTime.Parse("2024-06-03"),
                    RiskProjectId = 3,
                },
                new()
                {
                    Order = 4,
                    Name = "Konfigurační řízení, prezentace projektu",
                    Start = DateTime.Parse("2024-06-03"),
                    End = DateTime.Parse("2024-06-05"),
                    RiskProjectId = 3
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
