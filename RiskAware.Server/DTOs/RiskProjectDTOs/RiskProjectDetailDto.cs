namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    /// <summary>
    /// Data trasnfer object for detail tab of a RiskProject entity.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskProjectDetailDto
    {
        public int Id { get; set; } // TODO -> added this, check if rly needed
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public int Scale { get; set; } // TODO -> add to getDetail method in controller, will be used for choosing which matrix to use
    }
}
