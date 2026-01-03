using Logex.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logex.API.Data.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            builder.Property(c => c.IsActive).HasDefaultValue(true);

            builder
                .HasOne(c => c.Zone)
                .WithMany(z => z.Cities)
                .HasForeignKey(c => c.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
