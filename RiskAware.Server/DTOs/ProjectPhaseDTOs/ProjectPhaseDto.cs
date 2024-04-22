using RiskAware.Server.DTOs.RiskDTOs;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    /// <summary>
    /// DTO used for transferring project phase data between the server and the client.
    /// This DTO is used for displaying project phases in the phase pannel and tables.
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
