using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing a project phase entity in the database.
    /// </summary>
    /// <author> Dominik Pop </author>
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
        /// Foreign key to the risk project the phase belongs to.
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        ///// <summary>
        ///// Collection of risks associated with the project phase.
        ///// </summary>
        public ICollection<Risk> Risks { get; set; }

        ///// <summary>
        ///// Collection of project roles associated with the project phase.
        ///// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }
    }
}
