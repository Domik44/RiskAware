using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    public class ProjectPhaseCreateDto
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public RoleType UserRoleType { get; set; }
        public int RiskProjectId { get; set; }
    }
}
