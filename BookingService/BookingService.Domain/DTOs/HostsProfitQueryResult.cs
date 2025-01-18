namespace BookingService.Application.DTOs
{
    public record HostsProfitQueryResult
    {
        public string UserName { get; init; }
        public decimal TotalIncome { get; init; }
    }
}