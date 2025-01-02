namespace BookingService.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string HostId { get; set; }
        public string TenantId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }

        public User Host { get; set; }
        public User Tenant { get; set; }
        public Apartment Apartment { get; set; }
    }
}