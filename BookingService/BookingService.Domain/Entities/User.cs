using Microsoft.AspNetCore.Identity;

namespace BookingService.Domain.Entities
{
    public class User : IdentityUser
    {
        public string ExternalId { get; set; }
        public Wallet? Wallet { get; set; }
        public ICollection<Apartment> Apartments { get; set; }
        public ICollection<Booking> ClientBookings { get; set; }
        public IList<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
    }
}