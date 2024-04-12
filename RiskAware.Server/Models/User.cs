using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int AccountState { get; set; }
        public bool IsValid { get; set; }

        /// <summary>
        /// Many to one relationship, where one user is given one role.
        /// One-to-many without navigation to dependents
        /// </summary>
        [Required]
        public int SystemRoleId { get; set; }
        [ForeignKey(nameof(SystemRoleId))]
        public SystemRole SystemRole { get; set; }

        ///// <summary>
        ///// One to many relationship, where one user (admin) created this project.
        ///// </summary>
        public ICollection<RiskProject> RiskProjects { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }
    }
}
