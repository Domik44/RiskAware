using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace RiskAware.Server.Models
{
    public class ProjectRole
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public Guid RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        public int RoleType { get; set; } // TODO -> mozna udelat nejaky ciselnik s rolemi?
        public bool IsReqApproved { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public Guid ProjectPhaseId { get; set; } // TODO -> probrat s Dejvem
        //[ForeignKey(nameof(ProjectPhaseId))]
        //public virtual ProjectPhase ProjectPhase { get; set; }
    }
}
