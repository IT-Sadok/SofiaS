namespace BookingService.Application.Abstract
{
    public interface ICustomRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}