using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// Data transfer object for a page of a RiskProject entity.
    /// </summary>
    public class RiskProjectPageDto
    {
        public RiskProjectDetailDto Detail { get; set; }
        public IEnumerable<ProjectPhaseDto> Phases { get; set; }
        public IEnumerable<RiskDto> Risks { get; set; }
        public IEnumerable<ProjectRoleDto> Members { get; set; }
        public RoleType UserRole { get; set; }
        public ProjectPhaseSimpleDto AssignedPhase { get; set; }
    }
}
