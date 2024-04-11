using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// Data trasnfer object for detail tab of a RiskProject entity.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskProjectDetailDto
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<CommentDto> Comments { get; set; }

        public RiskProjectDetailDto(RiskProject riskProject)
        {
            Title = riskProject.Title;
            Description = riskProject.Description;
            Start = riskProject.Start;
            End = riskProject.End;
            Comments = riskProject.Comments.Select(c => new CommentDto(c)).ToList();
        }
    }
}
