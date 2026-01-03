using Logex.API.Dtos.ZoneDtos;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface IZoneService
    {
        Task<IEnumerable<Zone>> GetAllAsync();
        Task<Zone> GetByIdAsync(int id);
        Task<Zone> CreateZoneAsync(ZoneDto request);
        Task<Zone> UpdateZoneAsync(int id, ZoneDto request);
        Task<bool> ToggleStatusAsync(int id);
    }
}
