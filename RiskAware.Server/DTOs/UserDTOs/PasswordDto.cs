using System.ComponentModel.DataAnnotations;

namespace RiskAware.Server.DTOs.UserDTOs
{
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
