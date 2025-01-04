namespace BookingService.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }

        public string UserId { get; set; }
        public required User User { get; set; }
    }
}