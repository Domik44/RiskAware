using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public virtual RiskProject RiskProject { get; set; }

        // TODO -> v navrhu s otaznikem?
        //public Guid RiskId { get; set; }
        //[ForeignKey(nameof(RiskId))]
        //public virtual Risk Risk { get; set; }
    }
}
