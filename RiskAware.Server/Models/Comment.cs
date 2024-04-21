using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing comment entity in the database.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class Comment
    {
        public int Id { get; set; }
        [MinLength(1), MaxLength(1000)]
        public string Text { get; set; }
        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// Foreign key to the user who created the comment.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        ///// <summary>
        ///// Foreign key to the risk project the comment is associated with.
        ///// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }
    }
}
