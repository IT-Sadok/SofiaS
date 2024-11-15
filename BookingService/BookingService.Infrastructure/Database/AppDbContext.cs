using BookingService.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }

        public DbSet<User> Users { get; set; }
    }
}
