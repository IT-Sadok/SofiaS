namespace BookingService.Application.Abstract
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}