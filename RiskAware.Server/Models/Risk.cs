using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class Risk
    {
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        // TODO -> IsValid -> move from RiskHistory to Risk ??

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProjectPhaseId { get; set; }
        [ForeignKey("ProjectPhaseId")]
        public ProjectPhase ProjectPhase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RiskCategoryId { get; set; }
        [ForeignKey("RiskCathegoryId")]
        public RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<RiskHistory> RiskHistory { get; set; }
    }
}
