using RiskAware.Server.DTOs.RiskDTOs;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    /// <summary>
    /// This data transfer object represents a project phase listed in RiskProject table.
    /// </summary>
    public class ProjectPhaseDto
    {
        public int Id { get; set; }
        public int Order { get; set; } // TODO -> order by this
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<RiskUnderPhaseDto> Risks { get; set; }
    }
}
