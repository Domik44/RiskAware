namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// DTO used for transferring risk project data between the server and the client.
    /// This DTO is used for showing risk projects in the risk project table.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumOfMembers { get; set; }
        public string ProjectManagerName { get; set; }
        public bool IsValid { get; set; }
    }
}
