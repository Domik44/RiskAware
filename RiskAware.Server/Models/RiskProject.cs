using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskProject 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public bool IsValid { get; set; } // TODO -> nezapomenout, ze mazani se resi pomoci IsValid
        public int Scale { get; set; }
        public bool IsBlank { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectPhase> ProjectPhases { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Risk> Risks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<RiskCategory> RiskCategories { get; set; }
    }
}
