namespace RiskAware.Server.DTOs.UserDTOs
{
    /// <summary>
    /// DTO used for transferring user detail data between the server and the client.
    /// This DTO is used for displaying user details.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class UserDetailDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
