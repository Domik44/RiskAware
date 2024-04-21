using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing a risk category entity in the database.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class RiskCategory
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1), MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Foreign key to the risk project the category belongs to.
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        /// <summary>
        /// Collection of risks associated with the category.
        /// </summary>
        public ICollection<Risk> Risks { get; set; }
    }
}
