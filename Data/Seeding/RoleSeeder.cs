using Logex.API.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Data.Seeding
{
    public static class RoleSeeder
    {
        public static void SeedIdentityRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<IdentityRole<int>>()
                .HasData(
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = IdentityRoles.Admin,
                        NormalizedName = IdentityRoles.Admin.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = IdentityRoles.Customer,
                        NormalizedName = IdentityRoles.Customer.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    }
                );
        }
    }
}
