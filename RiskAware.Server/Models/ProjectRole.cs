using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

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
        public Guid ProjectRoleId { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsReqApproved { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public Guid ProjectPhaseId { get; set; }
        //[ForeignKey(nameof(ProjectPhaseId))]
        //public virtual ProjectPhase ProjectPhase { get; set; }
    }
}
