namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    /// <summary>
    /// DTO used for transferring project phase data between the server and the client.
    /// This DTO is used for getting users assigned phase.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class ProjectPhaseSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
