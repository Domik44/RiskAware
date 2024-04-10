namespace RiskAware.Server.DTOs
{
    public class RiskProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumOfMembers { get; set; }
        public string ProjectManagerName { get; set; }
    }
}
