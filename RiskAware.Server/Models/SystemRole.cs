using System.ComponentModel.DataAnnotations;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Class representing a system role entity in the database.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class SystemRole
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsAdministrator { get; set; }
        public string Description { get; set; }
    }
}
