namespace BookingService.Application.DTOs
{
    public record ApartmentUpsertDto
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}