using RiskAware.Server.DTOs.ProjectPhase;

namespace RiskAware.Server.DTOs.RiskProject
{
    /// <summary>
    /// Data transfer object for a page of a RiskProject entity.
    /// </summary>
    public class RiskProjectPageDto
    {
        public RiskProjectDetailDto RiskProjectDetail { get; set; }
        public IEnumerable<ProjectPhasesPanelDto> PhasesPanel { get; set; }
        public IEnumerable<ProjectPhaseDto> PhasesTable { get; set; }
        public IEnumerable<RiskDto> RisksTable { get; set; }
        public IEnumerable<RiskProjectMembersDto> MembersTable { get; set; }
    }
}
