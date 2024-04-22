namespace RiskAware.Server.DTOs.ProjectRoleDTOs
{
    /// <summary>
    /// DTO used for transferring project role data between the server and the client.
    /// This DTO is used for displaying project roles in table.
    /// </summary>
    /// <author>Dominik Pop</author>
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
