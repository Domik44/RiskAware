namespace RiskAware.Server.DTOs
{
    public class RiskProjectDetailDto
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
