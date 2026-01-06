using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Repository.Implementations
{
    public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(AppDbContext context)
            : base(context) { }

        public new async Task<Shipment> GetByIdAsync(int id)
        {
            return await _context
                .Shipments.Include(s => s.ShipmentMethod)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shipment> GetShipmentByTrackingNumberAsync(string trackingNumber)
        {
            return await _context
                .Shipments.Include(s => s.ShipmentMethod)
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);
        }
    }
}
