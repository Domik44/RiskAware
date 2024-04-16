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
        //public IEnumerable<ProjectPhasesPanelDto> PhasesPanel { get; set; } // TODO -> Delete
        public IEnumerable<ProjectPhaseDto> Phases { get; set; }
        public IEnumerable<RiskDto> Risks { get; set; }
        public IEnumerable<ProjectRoleDto> Members { get; set; }
        public bool IsAdmin { get; set; } // TODO -> delete this property
        public RoleType UserRole { get; set; }

        //public ProjectPhaseDto UserPhase { get; set; } // TODO -> will be used later
    }
}
