using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    /// <summary>
    /// DTO used for transferring project role data between the server and the client.
    /// This DTO is used for displaying project roles in table.
    /// </summary>
    public class ProjectRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsReqApproved { get; set; }
        public UserDto User { get; set; }
        public string ProjectPhaseName { get; set; }
    }
}
