using Logex.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logex.API.Data.Configurations
{
    public class Shipmentconfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasIndex(builder => builder.TrackingNumber).IsUnique();

            builder.Property(s => s.ShipperName).IsRequired().HasMaxLength(50);

            builder.Property(s => s.ShipperEmail).IsRequired().HasMaxLength(50);

            builder.Property(s => s.ShipperPhone).HasMaxLength(15);

            builder.Property(s => s.ShipperStreet).HasMaxLength(20);

            builder.Property(s => s.ShipperCityId).IsRequired();

            builder.Property(s => s.ReceiverName).IsRequired().HasMaxLength(20);

            builder.Property(s => s.ReceiverEmail).IsRequired().HasMaxLength(50);

            builder.Property(s => s.ReceiverPhone).HasMaxLength(15);

            builder.Property(s => s.ReceiverStreet).HasMaxLength(20);

            builder.Property(s => s.ReceiverCityId).IsRequired();

            builder.Property(s => s.Weight).HasPrecision(18, 2);

            builder.Property(s => s.Quantity).IsRequired();

            builder.Property(s => s.TotalCost).IsRequired();

            builder
                .HasOne(s => s.User)
                .WithMany(c => c.Shipments)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(s => s.ShipmentMethod)
                .WithMany(sm => sm.Shipments)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(s => s.Payment)
                .WithOne(p => p.Shipment)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(s => s.ShipperCity)
                .WithMany()
                .HasForeignKey(s => s.ShipperCityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.ReceiverCity)
                .WithMany()
                .HasForeignKey(s => s.ReceiverCityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
