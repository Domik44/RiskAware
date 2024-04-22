using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    /// <summary>
    /// DTO used for transferring project phase data between the server and the client.
    /// This DTO is used for creating a new project phase and editing old ones.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class ProjectPhaseCreateDto
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public RoleType UserRoleType { get; set; }
        public int RiskProjectId { get; set; }
    }
}
