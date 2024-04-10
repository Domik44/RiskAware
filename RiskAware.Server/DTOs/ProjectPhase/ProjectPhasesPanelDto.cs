using RiskAware.Server.DTOs.ProjectPhase;

namespace RiskAware.Server.DTOs
{
    /// <summary>
    /// This data transfer object represents a project phases showed in panel in RiskProject.
    /// </summary>
    public class ProjectPhasesPanelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RiskDto> Risks { get; set; }
    }
}
