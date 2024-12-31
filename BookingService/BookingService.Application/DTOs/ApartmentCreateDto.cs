namespace BookingService.Application.DTOs
{
    public record ApartmentCreateDto
    {
        public string? Name { get; init; }
        public decimal Price { get; init; }
        public string? Address { get; init; }
        public int RoomNumber { get; init; }
    }
}