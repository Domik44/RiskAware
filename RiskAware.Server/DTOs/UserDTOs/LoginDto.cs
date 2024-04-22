using System.ComponentModel.DataAnnotations;

namespace RiskAware.Server.DTOs.UserDTOs;

/// <summary>
/// DTO used for transferring login data between the server and the client.
/// This DTO is used for logging in a user.
/// </summary>
/// <author>Dominik Pop</author>
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
