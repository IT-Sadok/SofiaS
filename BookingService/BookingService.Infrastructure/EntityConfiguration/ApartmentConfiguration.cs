using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.ModelConfiguration
{
    internal class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.Address)
                .IsRequired();

            builder.Property(x => x.HostId)
                .IsRequired();

            builder.HasIndex(x => x.ExternalId)
                .IsUnique();

            builder
                 .HasOne(a => a.Host)
                 .WithMany(u => u.Apartments)
                 .HasForeignKey(a => a.HostId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
