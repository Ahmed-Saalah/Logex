using Logex.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logex.API.Data.Configurations
{
    public class ShipmentMethodConfiguration : IEntityTypeConfiguration<ShipmentMethod>
    {
        public void Configure(EntityTypeBuilder<ShipmentMethod> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Name).IsRequired().HasMaxLength(50);

            builder.Property(sm => sm.Cost).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(sm => sm.Duration).HasMaxLength(50);

            builder
                .HasMany(sm => sm.Shipments)
                .WithOne(s => s.ShipmentMethod)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
