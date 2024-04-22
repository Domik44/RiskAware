using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    /// <summary>
    /// DTO used for transferring project role data between the server and the client.
    /// This DTO is used for creating a new project role.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class ProjectRoleCreateDto
    {
        public string Name { get; set; }
        public RoleType RoleType { get; set; }
        public string Email { get; set; }
        public RoleType UserRoleType { get; set; }
        public int? ProjectPhaseId { get; set; }
    }
}
