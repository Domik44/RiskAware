using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class User : IdentityUser // TODO -> ted je Id jako string
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AccountState { get; set; }
        // TODO -> mozna pridat taky nejaky IsValid atribut pro "mazani" ?

        /// <summary>
        /// Many to one relationship, where one user is given one role.
        /// </summary>
        public Guid SystemRoleId { get; set; }
        [ForeignKey(nameof(SystemRoleId))]
        public virtual SystemRole SystemRole { get; set; }

        /// <summary>
        /// One to many relationship, where one user (admin) created this project.
        /// </summary>
        public ICollection<RiskProject> RiskProjects { get; set; }

        /// <summary>
        /// One to many relationship, where one user is creator of many comments.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Risk> Risks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<RiskHistory> RiskHistory { get; set; }
    }
}
