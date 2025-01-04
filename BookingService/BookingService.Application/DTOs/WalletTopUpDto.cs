namespace BookingService.Application.DTOs
{
    public record WalletTopUpDto
    {
        public decimal Amount { get; init; }
    }
}
