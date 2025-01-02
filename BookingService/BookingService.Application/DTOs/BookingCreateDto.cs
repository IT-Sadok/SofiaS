namespace BookingService.Application.DTOs
{
    public record BookingCreateDto
    {
        public int ApartmentId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}