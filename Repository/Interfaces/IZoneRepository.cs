using Logex.API.Models;

namespace Logex.API.Repository.Interfaces
{
    public interface IZoneRepository : IRepository<Zone>
    {
        Task<bool> IsZoneInUseAsync(int zoneId);
    }
}
