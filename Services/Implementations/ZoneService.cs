using Logex.API.Dtos.ZoneDtos;
using Logex.API.Models;
using Logex.API.Repository.Implementations;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepository;

        public ZoneService(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<IEnumerable<Zone>> GetAllAsync()
        {
            return await _zoneRepository.GetAllAsync();
        }

        public async Task<Zone> GetByIdAsync(int id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            return zone == null
                ? throw new KeyNotFoundException($"Zone with ID {id} not found.")
                : zone;
        }

        public async Task<Zone> CreateZoneAsync(ZoneDto request)
        {
            if (await _zoneRepository.ExistsAsync(_ => _.Name == request.Name))
            {
                throw new InvalidOperationException($"Zone '{request.Name}' already exists.");
            }

            var zone = new Zone { Name = request.Name };
            await _zoneRepository.AddAsync(zone);
            return zone;
        }

        public async Task<Zone> UpdateZoneAsync(int id, ZoneDto request)
        {
            var zone =
                await _zoneRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Zone with ID {id} not found.");

            if (!string.Equals(zone.Name, request.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _zoneRepository.ExistsAsync(_ => _.Name == request.Name))
                {
                    throw new InvalidOperationException($"Zone '{request.Name}' already exists.");
                }
            }

            zone.Name = request.Name;
            await _zoneRepository.UpdateAsync(zone);
            return zone;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var zone =
                await _zoneRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Zone not found.");

            zone.IsActive = !zone.IsActive;

            await _zoneRepository.UpdateAsync(zone);
            return zone.IsActive;
        }
    }
}
