using RiskAware.Server.DTOs.ProjectPhaseDTOs;
using RiskAware.Server.DTOs.ProjectRoleDTOs;
using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// DTO used for transferring risk project data between the server and the client.
    /// This DTO represents the whole risk project page. (excluding datables)
    /// </summary>
    /// <author>Dominik Pop</author>
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
