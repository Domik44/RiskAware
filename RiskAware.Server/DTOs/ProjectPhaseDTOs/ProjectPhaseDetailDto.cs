using RiskAware.Server.DTOs.RiskDTOs;
using RiskAware.Server.DTOs.UserDTOs;

namespace RiskAware.Server.DTOs.ProjectPhaseDTOs
{
    public class ProjectPhaseDetailDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<RiskDto> Risks { get; set; }
        // TODO -> assigned user 
        //public UserDto AssignedUser { get; set; }
    }
}
