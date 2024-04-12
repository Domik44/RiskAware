using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskProject
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1), MaxLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public bool IsValid { get; set; } // Used for soft deleting projects
        public int Scale { get; set; } // Is set by ProjectManager
        public bool IsBlank { get; set; } // Is set to false when admin creates project, then to true when ProjectManager starts it

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

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
