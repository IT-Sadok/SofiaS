namespace BookingService.Domain.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Host = "Host";
        public const string User = "User";

        public static readonly List<string> AllRoles = new List<string> { Admin, Host, User };

    }
}