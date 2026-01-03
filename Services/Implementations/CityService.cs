using Logex.API.Data;
using Logex.API.Dtos.CityDtos;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IZoneRepository _zoneRepository;

        public CityService(ICityRepository cityRepository, IZoneRepository zoneRepository)
        {
            _cityRepository = cityRepository;
            _zoneRepository = zoneRepository;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<City> GetByIdAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
                throw new KeyNotFoundException($"City with ID {id} not found.");
            return city;
        }

        public async Task<City> CreateCityAsync(CreateCityDto request)
        {
            if (await _cityRepository.ExistsAsync(_ => _.Name == request.Name))
            {
                throw new InvalidOperationException($"City '{request.Name}' already exists.");
            }

            var zoneExists = await _zoneRepository.ExistsAsync(z => z.ZoneId == request.ZoneId);
            if (!zoneExists)
            {
                throw new InvalidOperationException($"Zone ID {request.ZoneId} does not exist.");
            }

            var city = new City { Name = request.Name, ZoneId = request.ZoneId };

            await _cityRepository.AddAsync(city);
            return city;
        }

        public async Task<City> UpdateCityAsync(int id, UpdateCityDto request)
        {
            var city = await _cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                throw new KeyNotFoundException($"City with ID {id} not found.");
            }

            // Validate Zone if changed
            if (city.ZoneId != request.ZoneId)
            {
                var zoneExists = await _zoneRepository.ExistsAsync(z => z.ZoneId == request.ZoneId);
                if (!zoneExists)
                {
                    throw new InvalidOperationException(
                        $"Zone ID {request.ZoneId} does not exist."
                    );
                }
            }

            city.Name = request.Name;
            city.ZoneId = request.ZoneId;

            await _cityRepository.UpdateAsync(city);
            return city;
        }

        public async Task DeleteCityAsync(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null)
            {
                throw new KeyNotFoundException($"City with ID {id} not found.");
            }

            await _cityRepository.DeleteAsync(city.Id);
        }
    }
}
