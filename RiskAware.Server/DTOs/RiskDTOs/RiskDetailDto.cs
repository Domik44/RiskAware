namespace RiskAware.Server.DTOs.RiskDTOs
{
    public class RiskDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        // TODO -> PhaseName?
        public int Probability { get; set; }
        public int Impact { get; set; }
        public int Severity { get; set; }
        public string State { get; set; }
        public string Threat { get; set; }
        public string Indicators { get; set; }
        public string Prevention { get; set; }
        // TODO -> dates?
    }
}
