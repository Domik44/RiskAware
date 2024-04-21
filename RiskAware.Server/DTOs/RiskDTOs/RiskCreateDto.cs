using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.RiskDTOs
{
    public class RiskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Probability { get; set; }
        public int Impact { get; set; }
        public string Threat { get; set; }
        public string Indicators { get; set; }
        public string Prevention { get; set; }
        public string Status { get; set; }
        public DateTime? PreventionDone { get; set; }
        public DateTime? RiskEventOccured { get; set; }
        public DateTime End { get; set; }
        public int ProjectPhaseId { get; set; }
        public RiskCategoryDto RiskCategory { get; set; }
        public RoleType UserRoleType { get; set; }
    }
}
