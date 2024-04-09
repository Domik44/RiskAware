using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
