namespace BookingService.Domain.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; } //price per day
        public string? Address { get; set; }
        public int RoomNumber { get; set; }
        public bool IsAvailable { get; set; } = true;

        public required string HostId { get; set; }
        public required User Host { get; set; }
    }
}