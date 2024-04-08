using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public virtual RiskProject RiskProject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Risk> Risks { get; set; }
    }
}
