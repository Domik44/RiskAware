using System.ComponentModel.DataAnnotations;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// Model defining system role. System role represents users role in whole application.
    /// It's used to distinguish admin from basic user.
    /// </summary>
    /// <author>Dominik Pop</author>
    /// <date>05.04.2024</date>
    public class SystemRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public bool IsAdministrator { get; set; }
        public string Description { get; set; }
    }
}
