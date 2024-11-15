using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingService.Domain
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
    }
}
