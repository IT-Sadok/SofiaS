namespace BookingService.Domain.DTOs
{
    public record RepeatedBookingQueryResult
    {
        public int ApartmentId { get; init; }
    }
}
