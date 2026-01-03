using Logex.API.Dtos.ZoneRateDtos;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface IZoneRateService
    {
        Task<IEnumerable<ZoneRate>> GetAllAsync();
        Task<ZoneRate> GetByIdAsync(int id);
        Task<ZoneRate> CreateRateAsync(CreateZoneRateDto request);
        Task<ZoneRate> UpdateRateAsync(int id, UpdateZoneRateDto request);
        Task DeleteRateAsync(int id);
    }
}
