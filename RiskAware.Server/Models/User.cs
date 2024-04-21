using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing a user entity in the database.
    /// Inherits from IdentityUser class.
    /// </summary>
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int AccountState { get; set; }
        public bool IsValid { get; set; }

        /// <summary>
        /// Foreign key to the system role the user belongs to.
        /// </summary>
        [Required]
        public int SystemRoleId { get; set; }
        [ForeignKey(nameof(SystemRoleId))]
        public SystemRole SystemRole { get; set; }

        /// <summary>
        /// Collection of risk projects created by the user.
        /// </summary>
        public ICollection<RiskProject> RiskProjects { get; set; }

        /// <summary>
        /// Collection of ProjectRoles associated with the user.
        /// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }
    }
}
