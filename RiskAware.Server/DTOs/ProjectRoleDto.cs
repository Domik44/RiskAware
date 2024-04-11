using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs
{
    public class ProjectRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } // TODO -> depends on how I decided in model
        public bool IsReqApproved { get; set; }
        public UserDto User { get; set; }

        // todo nejak include faze -> asi predelat modely

        //public ProjectRoleDto(RiskProject riskProject)
        //{

        //}
    }
}
