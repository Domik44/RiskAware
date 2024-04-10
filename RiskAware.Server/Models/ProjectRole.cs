using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public enum RoleType
    {
        ProjectManager = 0,
        RiskManager = 1,
        TeamMember = 2,
        ExternalMember = 3,
        CommonUser = 4
    }

    public class ProjectRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsReqApproved { get; set; }
        // TODO -> maybe add name of role which would be set by user

        /// <summary>
        /// 
        /// </summary>
        //public int ProjectPhaseId { get; set; }
        //[ForeignKey(nameof(ProjectPhaseId))]
        //public ProjectPhase ProjectPhase { get; set; }
    }
}
