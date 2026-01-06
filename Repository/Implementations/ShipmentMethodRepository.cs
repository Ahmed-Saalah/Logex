using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Repository.Implementations
{
    public class ShipmentMethodRepository : Repository<ShipmentMethod>, IShipmentMethodRepository
    {
        public ShipmentMethodRepository(AppDbContext context)
            : base(context) { }

        public override async Task<IEnumerable<ShipmentMethod>> GetAllAsync()
        {
            return await _context
                .ShipmentMethods.Where(_ => _.IsActive)
                .Include(_ => _.Shipments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ShipmentMethod?> GetShipmentMethodByNameAsync(string name)
        {
            return await _context.ShipmentMethods.FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task<ShipmentMethod> GetAsync(Func<ShipmentMethod, bool> predicate)
        {
            return await Task.Run(() => _context.ShipmentMethods.FirstOrDefault(predicate));
        }

        public async Task<ShipmentMethod> GetByIdAsync(int id)
        {
            var entity = await _context.Set<ShipmentMethod>().FindAsync(id);
            return entity;
        }
    }
}
