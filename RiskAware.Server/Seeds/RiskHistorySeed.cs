using RiskAware.Server.Data;
using RiskAware.Server.Models;

namespace RiskAware.Server.Seeds
{
    public class RiskHistorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var riskHistoryToBeAdded = new RiskHistory[]
            {
                new()
                {
                    Title = "Riziko 1",
                    Description = "Popis rizika 1",
                    Probability = 1,
                    Impact = 2,
                    Threat = "Hrozba",
                    Indicators = "Nejaky indikator",
                    Prevention = "Nejaka ochrana",
                    PreventionDone = DateTime.Parse("2024-05-21 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("2024-05-21 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("2024-05-20 12:00:00"),
                    Created = DateTime.Parse("2024-05-20 12:00:00"),
                    StatusLastModif = DateTime.Parse("2024-05-20 12:00:00"),
                    End = DateTime.Parse("2024-05-23 12:00:00"),
                    RiskId = 1,
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                },
                new()
                {
                    Title = "Riziko 2",
                    Description = "Popis rizika 2",
                    Probability = 2,
                    Impact = 2,
                    Threat = "Hrozba 2",
                    Indicators = "Nejaky indikator 2",
                    Prevention = "Nejaka ochrana 2",
                    PreventionDone = DateTime.Parse("2024-05-22 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("2024-05-22 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("2024-05-22 18:00:00"),
                    Created = DateTime.Parse("2024-05-21 12:00:00"),
                    StatusLastModif = DateTime.Parse("2024-05-21 12:00:00"),
                    End = DateTime.Parse("2024-05-24 12:00:00"),
                    RiskId = 2,
                    UserId = "84c8b270-14e5-4158-bcde-a76c6edc4cf7",
                },
                new()
                {
                    Title = "Riziko 3",
                    Description = "Popis rizika 3",
                    Probability = 4,
                    Impact = 5,
                    Threat = "Hrozba",
                    Indicators = "Nejaky indikator",
                    Prevention = "Nejaka ochrana",
                    PreventionDone = DateTime.Parse("2024-05-25 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("2024-05-25 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("2024-05-25 12:00:00"),
                    Created = DateTime.Parse("2024-05-24 12:00:00"),
                    StatusLastModif = DateTime.Parse("2024-05-25 12:00:00"),
                    End = DateTime.Parse("2024-05-26 12:00:00"),
                    RiskId = 3,
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                },
                new()
                {
                    Title = "Riziko 4",
                    Description = "Popis rizika 4",
                    Probability = 4,
                    Impact = 5,
                    Threat = "Hrozba",
                    Indicators = "Nejaky indikator",
                    Prevention = "Nejaka ochrana",
                    PreventionDone = DateTime.Parse("2024-05-28 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("2024-05-27 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("2024-05-25 12:00:00"),
                    Created = DateTime.Parse("2024-05-25 12:00:00"),
                    StatusLastModif = DateTime.Parse("2024-05-25 12:00:00"),
                    End = DateTime.Parse("2024-05-29 12:00:00"),
                    RiskId = 4,
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                },
                new()
                {
                    Title = "Riziko 3",
                    Description = "Popis rizika 3",
                    Probability = 4,
                    Impact = 5,
                    Threat = "Hrozba",
                    Indicators = "Nejaky indikator",
                    Prevention = "Nejaka ochrana",
                    PreventionDone = DateTime.Parse("2024-05-28 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("2024-05-27 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("2024-05-26 12:00:00"),
                    Created = DateTime.Parse("2024-05-26 12:00:00"),
                    StatusLastModif = DateTime.Parse("2024-05-26 12:00:00"),
                    End = DateTime.Parse("2024-05-29 12:00:00"),
                    RiskId = 5,
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                }
            };

            foreach (var riskHistory in riskHistoryToBeAdded)
            {
                if (!context.RiskHistory.Any(h => h.Id == riskHistory.Id))
                {
                    context.RiskHistory.Add(riskHistory);
                }
            }
            context.SaveChanges();
        }
    }
}
