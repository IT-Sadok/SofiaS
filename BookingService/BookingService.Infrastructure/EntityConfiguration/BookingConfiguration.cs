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
                .HasOne(b => b.Client)
                .WithMany(u => u.ClientBookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
            builder
                .HasOne(b => b.Apartment)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
