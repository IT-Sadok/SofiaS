namespace BookingService.Domain.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; } //price per day
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public string HostId { get; set; }
        public string? CustomData { get; set; } //json

        public User Host { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}