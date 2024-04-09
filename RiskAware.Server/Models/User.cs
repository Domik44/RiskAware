using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    [Table("Users")]
    public class User : IdentityUser // TODO -> ted je Id jako string
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
        // This would be used for lazy loading -> we dont use it
        //public virtual SystemRole SystemRole { get; set; }

        ///// <summary>
        ///// One to many relationship, where one user (admin) created this project.
        ///// </summary>
        public ICollection<RiskProject> RiskProjects { get; set; }

        ///// <summary>
        ///// One to many relationship, where one user is creator of many comments.
        ///// </summary>
        public ICollection<Comment> Comments { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<Risk> Risks { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<RiskHistory> RiskHistory { get; set; }
    }
}
