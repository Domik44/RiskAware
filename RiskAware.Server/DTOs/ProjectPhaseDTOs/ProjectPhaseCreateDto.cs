using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    public class ProjectPhaseCreateDto
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        //public int ProjectRoleId { get; set; } // TODO -> when user is assigned to phase
        public RoleType UserRoleType { get; set; }
    }
}
