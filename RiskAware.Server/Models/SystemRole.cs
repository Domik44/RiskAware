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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdministrator { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// One to Many relationship, where one role is assigned to one user.
        /// </summary>
        public ICollection<User> Users { get; set; }
    }
}
