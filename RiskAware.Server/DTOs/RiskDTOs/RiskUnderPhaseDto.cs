namespace RiskAware.Server.DTOs.RiskDTOs
{
    /// <summary>
    /// DTO used for transferring risk data between the server and the client.
    /// This DTO is used for displaying risks in phase pannel.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskUnderPhaseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
