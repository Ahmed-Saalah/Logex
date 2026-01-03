using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Repository.Implementations
{
    public class ZoneRateRepository : Repository<ZoneRate>, IZoneRateRepository
    {
        public ZoneRateRepository(AppDbContext context)
            : base(context) { }

        public async Task<IEnumerable<ZoneRate>> GetAllWithZonesAsync()
        {
            return await _context
                .ZoneRates.Include(r => r.FromZone)
                .Include(r => r.ToZone)
                .OrderBy(r => r.FromZone.Name)
                .ThenBy(r => r.ToZone.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsRouteAsync(int fromZoneId, int toZoneId)
        {
            return await _context.ZoneRates.AnyAsync(r =>
                r.FromZoneId == fromZoneId && r.ToZoneId == toZoneId
            );
        }

        public async Task<ZoneRate?> GetByRouteAsync(int fromZoneId, int toZoneId)
        {
            return await _context.ZoneRates.FirstOrDefaultAsync(r =>
                r.FromZoneId == fromZoneId && r.ToZoneId == toZoneId
            );
        }

        public override async Task<ZoneRate?> GetByIdAsync(int id)
        {
            return await _context
                .ZoneRates.Include(r => r.FromZone)
                .Include(r => r.ToZone)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
