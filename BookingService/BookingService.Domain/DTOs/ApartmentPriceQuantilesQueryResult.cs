namespace BookingService.Domain.DTOs
{
    public record ApartmentPriceQuantilesQueryResult
    {
        public int ApartmentId { get; init; }
        public decimal Price { get; init; }
        public int Quantile { get; init; }
    }
}
