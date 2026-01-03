using Logex.API.Data;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logex.API.Repository.Implementations
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context)
            : base(context) { }

        public override async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Cities.Include(c => c.Zone).OrderBy(c => c.Name).ToListAsync();
        }

        public override async Task<City?> GetByIdAsync(int id)
        {
            return await _context.Cities.Include(c => c.Zone).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
