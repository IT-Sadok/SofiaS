using Microsoft.AspNetCore.Identity;

namespace BookingService.Domain.Entities
{
    public class User : IdentityUser
    {
        public Wallet? Wallet { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Booking> TenantBookings { get; set; }
        public ICollection<Booking> HostBookings { get; set; }
    }
}