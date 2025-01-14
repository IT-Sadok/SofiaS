using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntityConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(w => w.Wallet)
                .WithOne(u => u.User)
                .HasForeignKey<Wallet>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(u => u.UserId);

            //builder.HasIndex(x => x.ExternalId)
            //    .IsUnique();
        }
    }
}