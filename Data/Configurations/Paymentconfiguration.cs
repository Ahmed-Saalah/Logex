using Logex.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logex.API.Data.Configurations
{
    public class Paymentconfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount).IsRequired().HasPrecision(18, 2);

            builder
                .HasOne(p => p.Shipment)
                .WithOne(s => s.Payment)
                .HasForeignKey<Shipment>(s => s.PaymentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
