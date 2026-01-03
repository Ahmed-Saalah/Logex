using Logex.API.Dtos.ShipmentMethodDtos;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface IShipmentMethodService
    {
        Task<ShipmentMethod> GetByIdAsync(int id);

        Task<decimal> GetShipmentMethodCostAsync(int id);

        Task<IEnumerable<ShipmentMethod>> GetAllAsync();

        Task<ShipmentMethod> CreateMethodAsync(CreateShipmentMethodDto request);

        Task<ShipmentMethod> UpdateMethodAsync(int id, UpdateShipmentMethodDto request);

        Task<bool> ToggleStatusAsync(int id);
    }
}
