namespace RiskAware.Server.DTOs.ProjectPhase
{
    /// <summary>
    /// This data transfer object represents a project phase listed in RiskProject table.
    /// </summary>
    public class ProjectPhaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        // TODO -> Add more properties
    }
}
