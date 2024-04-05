using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectPhase
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public virtual RiskProject RiskProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Risk> Risks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public ICollection<ProjectRole> ProjectRoles { get; set; } // TODO -> probrat s Dejvem
    }
}
