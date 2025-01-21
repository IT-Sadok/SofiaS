namespace BookingService.Domain.DTOs
{
    public record BookingDurationQueryResult
    {
        public int ApartmentId { get; set; }
        public int AverageDuration { get; set; }
    }
}
