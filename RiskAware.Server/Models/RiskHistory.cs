using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskHistory
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Range(1, 5)]
        public int Probability { get; set; }
        [Range(1, 5)]
        public int Impact { get; set; }
        public string Threat { get; set; }
        public string Indicators { get; set; }
        public string Prevention { get; set; }
        public DateTime PreventionDone { get; set; }
        public string Status { get; set; }
        public DateTime RiskEventOccured { get; set; }
        public bool IsApproved { get; set; }
        public bool IsValid { get; set; }
        public DateTime LastModif { get; set; } // Does system
        public DateTime Created { get; set; } // Does system
        public DateTime StatusLastModif { get; set; } // Does system
        public DateTime End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RiskId { get; set; }
        [ForeignKey(nameof(RiskId))]
        public Risk Risk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
