using Logex.API.Dtos.CityDtos;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City> GetByIdAsync(int id);
        Task<City> CreateCityAsync(CreateCityDto request);
        Task<City> UpdateCityAsync(int id, UpdateCityDto request);
        Task DeleteCityAsync(int id);
    }
}
