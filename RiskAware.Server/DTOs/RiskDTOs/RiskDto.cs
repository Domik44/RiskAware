namespace RiskAware.Server.DTOs.RiskDTOs
{
    /// <summary>
    /// This data transfer object represents a risk listed in RiskProject phases pannel and table.
    /// </summary>
    public class RiskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public int Severity { get; set; }
        public int Probability { get; set; }
        public int Impact { get; set; }
        public string State { get; set; }
    }
}
