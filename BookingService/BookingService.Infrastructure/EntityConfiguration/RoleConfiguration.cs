using BookingService.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntityConfiguration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Name = Roles.Admin, NormalizedName = Roles.Admin.ToUpper() },
                new IdentityRole { Name = Roles.Host, NormalizedName = Roles.Host.ToUpper() },
                new IdentityRole { Name = Roles.User, NormalizedName = Roles.User.ToUpper() }
            );
        }
    }
}