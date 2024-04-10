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
                    PreventionDone = DateTime.Parse("21/05/2024 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("21/05/2024 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("20/05/2024 12:00:00"),
                    Created = DateTime.Parse("20/05/2024 12:00:00"),
                    StatusLastModif = DateTime.Parse("20/05/2024 12:00:00"),
                    End = DateTime.Parse("23/05/2024 12:00:00"),
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
                    PreventionDone = DateTime.Parse("22/05/2024 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("22/05/2024 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("22/05/2024 18:00:00"),
                    Created = DateTime.Parse("21/05/2024 12:00:00"),
                    StatusLastModif = DateTime.Parse("21/05/2024 12:00:00"),
                    End = DateTime.Parse("24/05/2024 12:00:00"),
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
                    PreventionDone = DateTime.Parse("25/05/2024 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("25/05/2024 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("25/05/2024 12:00:00"),
                    Created = DateTime.Parse("24/05/2024 12:00:00"),
                    StatusLastModif = DateTime.Parse("25/05/2024 12:00:00"),
                    End = DateTime.Parse("26/05/2024 12:00:00"),
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
                    PreventionDone = DateTime.Parse("28/05/2024 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("27/05/2024 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("25/05/2024 12:00:00"),
                    Created = DateTime.Parse("25/05/2024 12:00:00"),
                    StatusLastModif = DateTime.Parse("25/05/2024 12:00:00"),
                    End = DateTime.Parse("29/05/2024 12:00:00"),
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
                    PreventionDone = DateTime.Parse("28/05/2024 18:00:00"),
                    Status = "Nejaky status rizika",
                    RiskEventOccured = DateTime.Parse("27/05/2024 12:00:00"),
                    IsApproved = true,
                    IsValid = true,
                    LastModif = DateTime.Parse("26/05/2024 12:00:00"),
                    Created = DateTime.Parse("26/05/2024 12:00:00"),
                    StatusLastModif = DateTime.Parse("26/05/2024 12:00:00"),
                    End = DateTime.Parse("29/05/2024 12:00:00"),
                    RiskId = 5,
                    UserId = "5862be25-6467-450e-81fa-1cac9578650b",
                }
            };

            foreach(var riskHistory in riskHistoryToBeAdded)
            {
                if(!context.RiskHistory.Any(h => h.Id == riskHistory.Id))
                {
                    context.RiskHistory.Add(riskHistory);
                }
            }
            context.SaveChanges();
        }
    }
}
