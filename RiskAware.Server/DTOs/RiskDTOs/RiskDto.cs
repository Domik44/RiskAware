using RiskAware.Server.DTOs.ProjectPhaseDTOs;

namespace RiskAware.Server.DTOs.RiskDTOs
{
    /// <summary>
    /// DTO used for transferring risk data between the server and the client.
    /// This DTO is used for displaying risks in table.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public int Severity { get; set; }
        public int Probability { get; set; }
        public int Impact { get; set; }
        public string State { get; set; }
        public ProjectPhaseSimpleDto ProjectPhase { get; set; }
    }
}
