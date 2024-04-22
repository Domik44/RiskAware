namespace RiskAware.Server.DTOs
{
    /// <summary>
    /// DTO used for transferring risk category data between the server and the client.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class RiskCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
