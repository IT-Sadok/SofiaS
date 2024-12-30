using Constants = BookingService.Domain.Constants;
using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {  
        }

        public DbSet<Apartment> Apartments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(InfrastructureAssembly).Assembly);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = Constants.Roles.Admin, NormalizedName = Constants.Roles.Admin.ToUpper() },
                new IdentityRole { Name = Constants.Roles.Host, NormalizedName = Constants.Roles.Host.ToUpper() },
                new IdentityRole { Name = Constants.Roles.User, NormalizedName = Constants.Roles.User.ToUpper() }
            );
        }
    }
}