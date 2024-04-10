using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectPhase
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RiskProjectId { get; set; }
        [ForeignKey(nameof(RiskProjectId))]
        public RiskProject RiskProject { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public ICollection<Risk> Risks { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        public int? ProjectRoleId { get; set; }
        [ForeignKey(nameof(ProjectRoleId))]
        public ProjectRole ProjectRole { get; set; }
    }
}
