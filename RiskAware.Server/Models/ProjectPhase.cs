using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectPhase
    {
        public int Id { get; set; }
        public int Order { get; set; }
        [Required]
        [MinLength(1), MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<Risk> Risks { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public int? ProjectRoleId { get; set; }
        [ForeignKey(nameof(ProjectRoleId))]
        public ProjectRole ProjectRole { get; set; }
    }
}
