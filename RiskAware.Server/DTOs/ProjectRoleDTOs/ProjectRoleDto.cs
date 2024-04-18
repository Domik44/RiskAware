using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    public class ProjectRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsReqApproved { get; set; }
        public UserDto User { get; set; }
        public string ProjectPhaseName { get; set; }
    }
}
