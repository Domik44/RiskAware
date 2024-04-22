namespace RiskAware.Server.DTOs.UserDTOs
{
    /// <summary>
    /// DTO used for transferring user data between the server and the client.
    /// This DTO is used when basic user information is needed.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
