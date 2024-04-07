using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs
{
    public class SystemRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdministrator { get; set; }
        public string Description { get; set; }

        public SystemRoleDto(SystemRole systemRole)
        {
            Id = systemRole.Id;
            Name = systemRole.Name;
            IsAdministrator = systemRole.IsAdministrator;
            Description = systemRole.Description;
        }
    }
}

