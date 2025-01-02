using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.EntityConfiguration
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder
                .HasOne(b => b.Host)
                .WithMany(u => u.HostBookings)
                .HasForeignKey(b => b.HostId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(b => b.Tenant)
                .WithMany(u => u.TenantBookings)
                .HasForeignKey(b => b.TenantId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(b => b.Apartment)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
