namespace RiskAware.Server.DTOs.RiskProject
{
    /// <summary>
    /// Data transfer object for a page of a RiskProject entity.
    /// </summary>
    public class RiskProjectPageDto
    {
        public RiskProjectDetailDto RiskProjectDetail { get; set; }
        //public ICollection<RiskProjectPhasesDto> Phases { get; set; }
        public ProjectPhasesPanelDto PhasesPanel { get; set; }
    }
}
