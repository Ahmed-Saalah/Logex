using Logex.API.Models;

namespace Logex.API.Repository.Interfaces
{
    public interface IZoneRateRepository : IRepository<ZoneRate>
    {
        Task<IEnumerable<ZoneRate>> GetAllWithZonesAsync();

        Task<bool> ExistsRouteAsync(int fromZoneId, int toZoneId);

        Task<ZoneRate?> GetByRouteAsync(int fromZoneId, int toZoneId);
    }
}
