using System.ComponentModel.DataAnnotations.Schema;

namespace RiskAware.Server.Models
{
    public class RiskProject 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public bool IsValid { get; set; } // TODO -> nezapomenout, ze mazani se resi pomoci IsValid
        // TODO -> atribut pro nastaveni stupne te skaly?
        public int Scale { get; set; }
        // TODO -> taky atribut na oznaceni toho blank stavu
        //public bool IsBlank { get; set; }

        // TODO -> vyresit jak to teda bude se vztahem projektu a usera skrz toho admina a projektaka
        // Momentalne se zde jedna o zakladatele projektu takze admina
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectPhase> ProjectPhases { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<ProjectRole> ProjectRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Risk> Risks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public ICollection<RiskCategory> RiskCategories { get; set; } // TODO -> pobavit se s dejvem
    }
}
