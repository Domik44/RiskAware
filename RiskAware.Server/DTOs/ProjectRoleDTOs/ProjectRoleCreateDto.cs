using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    public class ProjectRoleCreateDto
    {
        public string Name { get; set; }
        public RoleType RoleType { get; set; }
        public string UserId { get; set; }
        //public int RiskProjectId { get; set; }
        public int? ProjectPhaseId { get; set; }
    }
}
