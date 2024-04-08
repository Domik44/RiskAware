using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class Risk
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public virtual RiskProject RiskProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectPhaseId { get; set; }
        [ForeignKey("ProjectPhaseId")]
        public virtual ProjectPhase ProjectPhase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskCathegoryId { get; set; }
        [ForeignKey("RiskCathegoryId")]
        public virtual RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<RiskHistory> RiskHistory { get; set; }
    }
}
