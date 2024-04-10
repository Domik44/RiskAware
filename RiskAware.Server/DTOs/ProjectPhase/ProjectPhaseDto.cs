namespace RiskAware.Server.DTOs.ProjectPhase
{
    /// <summary>
    /// This data transfer object represents a project phase listed in RiskProject phases pannel.
    /// </summary>
    public class ProjectPhaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RiskDto> Risks { get; set; }
    }
}
