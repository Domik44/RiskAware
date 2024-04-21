using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public enum RoleType
    {
        ProjectManager = 0,
        RiskManager = 1,
        TeamMember = 2,
        ExternalMember = 3,
        CommonUser = 4 // This means he user has no role assigned to the project
    }

    /// <summary>
    /// Class representing a project role entity in the database.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class ProjectRole
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1), MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public RoleType RoleType { get; set; }
        public bool IsReqApproved { get; set; }

        /// <summary>
        /// Foreign key to the user having this role on a project.
        /// </summary>
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// Foreign key to the risk project the role belongs to.
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        /// <summary>
        /// Foreign key to the project phase the role belongs to.
        /// Only assigned if the role is of type TeamMember.
        /// </summary>
        public int? ProjectPhaseId { get; set; }
        [ForeignKey(nameof(ProjectPhaseId))]
        public ProjectPhase ProjectPhase { get; set; }
    }
}
