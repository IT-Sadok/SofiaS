namespace BookingService.Domain.Interfaces
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}