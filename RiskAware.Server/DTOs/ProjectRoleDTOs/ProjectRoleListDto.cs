namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    public class ProjectRoleListDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProjectPhaseName { get; set; }
        public bool IsReqApproved { get; set; }
    }
}
