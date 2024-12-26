using System.Runtime.CompilerServices;

namespace BookingService.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidationMiddleware>();
        }
    }
}
