namespace BookingService.Application.DTOs
{
    public class ApartmentCreateDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Address { get; set; }
        public int RoomNumber { get; set; }
    }
}