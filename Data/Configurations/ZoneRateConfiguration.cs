using Logex.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logex.API.Data.Configurations
{
    public class ZoneRateConfiguration : IEntityTypeConfiguration<ZoneRate>
    {
        public void Configure(EntityTypeBuilder<ZoneRate> builder)
        {
            builder.HasKey(zr => zr.Id);

            builder.HasIndex(zr => new { zr.FromZoneId, zr.ToZoneId }).IsUnique();

            builder.Property(zr => zr.BaseRate).IsRequired().HasPrecision(18, 2);

            builder.Property(zr => zr.AdditionalWeightCost).HasPrecision(18, 2);

            builder
                .HasOne(zr => zr.FromZone)
                .WithMany()
                .HasForeignKey(zr => zr.FromZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(zr => zr.ToZone)
                .WithMany()
                .HasForeignKey(zr => zr.ToZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
