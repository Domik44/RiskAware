using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.DTOs.RiskProjectDTOs
{
    public class RiskProjectCreateDto
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Email { get; set; }
        //public UserDto ProjectManager { get; set; }
    }
}
