using System.ComponentModel.DataAnnotations;

namespace RiskAware.Server.DTOs.UserDTOs
{
    /// <summary>
    /// DTO used for transferring password data between the server and the client.
    /// This DTO is used for changing a user's password.
    /// </summary>
    /// <author>Dominik Pop</author>
    public class PasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } 
    }
}
