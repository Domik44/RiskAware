using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    public class ProjectRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } // TODO -> depends on how I decided in model
        public bool IsReqApproved { get; set; }
        public UserDto User { get; set; }
        public string ProjectPhaseName { get; set; }
    }
}
