using Microsoft.AspNetCore.Identity;

namespace BookingService.Domain
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
    }
}
