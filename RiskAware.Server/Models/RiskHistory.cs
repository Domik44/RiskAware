using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskHistory
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Probability { get; set; } // TODO -> vazne jako int??
        public int Impact { get; set; }
        public string Threat { get; set; }
        public string Indicators { get; set; }
        public string Prevention { get; set; }
        public DateTime PreventionDone { get; set; }
        public string Status { get; set; }
        public DateTime RiskEventOccured { get; set; }
        public bool IsApproved { get; set; }
        public bool IsValid { get; set; }
        public DateTime LastModif { get; set; }
        public DateTime Created { get; set; }
        public DateTime StatusLastModif { get; set; }
        public DateTime End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RiskId { get; set; } // TODO -> doopravdy chci mazat kdyz se smaze riziko, nechci tam tu historii ponechat -> bylo by potreba ukladat vsechny atributy rizika
        [ForeignKey(nameof(RiskId))]
        public virtual Risk Risk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
