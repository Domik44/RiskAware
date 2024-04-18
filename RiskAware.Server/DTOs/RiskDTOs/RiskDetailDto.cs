﻿namespace RiskAware.Server.DTOs.RiskDTOs
{
    public class RiskDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Probability { get; set; }
        public int Severity { get; set; }
        public int Impact { get; set; }
        public string Threat { get; set; }
        public string Indicators { get; set; }
        public string Prevention { get; set; }
        public string Status { get; set; }
        public DateTime PreventionDone { get; set; }
        public DateTime RiskEventOccured { get; set; }
        public DateTime End { get; set; }
        public DateTime LastModif { get; set; }
        public DateTime Created { get; set; }
        public DateTime StatusLastModif { get; set; }

        public string ProjectPhaseName { get; set; }
        public string RiskCategoryName { get; set; }
        public bool IsVaid { get; set; }
        public bool IsApproved { get; set; }
        public string UserFullName { get; set; }

    }
}
