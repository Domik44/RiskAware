using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MinLength(1), MaxLength(1000)]
        public string Text { get; set; }
        [Required]
        public DateTime Created { get; set; } // TODO -> pridat do migraci

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }
    }
}
