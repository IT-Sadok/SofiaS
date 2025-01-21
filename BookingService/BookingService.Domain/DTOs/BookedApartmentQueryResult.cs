namespace BookingService.Domain.DTOs
{
    public record BookedApartmentQueryResult
    {
        public int ApartmentId { get; init; }
        public int BookingCount { get; init; }
    }
}
