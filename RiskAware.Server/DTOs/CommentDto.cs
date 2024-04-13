using RiskAware.Server.Models;

namespace RiskAware.Server.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
    }
}
