namespace RiskAware.Server.DTOs
{
    /// <summary>
    /// DTO used for transferring comment data between the server and the client.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
    }
}
