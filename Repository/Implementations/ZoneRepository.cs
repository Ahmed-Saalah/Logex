using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Repository.Implementations
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(AppDbContext context)
            : base(context) { }

        public async Task<bool> IsZoneInUseAsync(int zoneId)
        {
            return await _context.Cities.AnyAsync(c => c.ZoneId == zoneId)
                || await _context.ZoneRates.AnyAsync(r =>
                    r.FromZoneId == zoneId || r.ToZoneId == zoneId
                );
        }
    }
}
