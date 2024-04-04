using Microsoft.AspNetCore.Identity;

namespace RiskAware.Server.Models
{
    public class User : IdentityUser
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
