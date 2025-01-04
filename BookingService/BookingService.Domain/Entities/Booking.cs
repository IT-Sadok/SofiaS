namespace BookingService.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }

        public User Client { get; set; }
        public Apartment Apartment { get; set; }
    }
}