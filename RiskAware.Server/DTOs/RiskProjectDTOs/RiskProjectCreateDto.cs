namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// DTO used for transferring risk project data between the server and the client.
    /// This DTO is used when creating a new risk project.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskProjectCreateDto
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Email { get; set; }
    }
}
