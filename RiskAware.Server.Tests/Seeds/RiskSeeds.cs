using RiskAware.Server.DTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.Tests.Seeds
{
    public class RiskSeeds
    {
        public static readonly RiskCreateDto RiskCreateDto = new()
        {
            Title = "Riziko_Test",
            Description = "Test description",
            Probability = 3,
            Impact = 1,
            Threat = "Testovací hrozba",
            Indicators = "Testovací indikátory",
            Prevention = "Testovaci prevence",
            Status = "Aktivní",
            PreventionDone = DateTime.Now,
            RiskEventOccured = DateTime.Now,
            End = DateTime.Now.AddDays(10),
            ProjectPhaseId = 1,
            RiskCategory = new RiskCategoryDto {Id = 1, Name = "Testovací kategorie"},
            UserRoleType = RoleType.ProjectManager
        };
    }
}
