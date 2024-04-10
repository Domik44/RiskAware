namespace RiskAware.Server.DTOs
{
    public class ProjectPhasesPanelDto
    {
        //public ProjectPhaseDto[] ProjectPhases { get; set; }
        public ICollection<ProjectPhaseDto> ProjectPhases { get; set; }
    }
}
