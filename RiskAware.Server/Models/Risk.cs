using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing a risk entity in the database.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class Risk
    {
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Foreign key to the user who created the risk.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// Foreign key to the risk project the risk belongs to.
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        /// <summary>
        /// Foreign key to the project phase the risk belongs to.
        /// </summary>
        public int ProjectPhaseId { get; set; }
        [ForeignKey("ProjectPhaseId")]
        public ProjectPhase ProjectPhase { get; set; }

        /// <summary>
        /// Foreign key to the risk category the risk belongs to.
        /// </summary>
        public int RiskCategoryId { get; set; }
        [ForeignKey("RiskCathegoryId")]
        public RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// Collection of risk history entries associated with the risk.
        /// </summary>
        public ICollection<RiskHistory> RiskHistory { get; set; }
    }
}
