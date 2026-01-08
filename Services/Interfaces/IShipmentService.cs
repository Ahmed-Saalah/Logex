using Logex.API.Common;
using Logex.API.Dtos.ShipmentDtos;
using Logex.API.Models;

namespace Logex.API.Services.Interfaces
{
    public interface IShipmentService
    {
        Task<Shipment> CreateShipmentAsync(CreateShipmentDto shipmentCreateDTO, int customerId);

        Task<Shipment> GetByIdAsync(int id);

        Task<Shipment> GetByTrackingNumberAsync(string trackingNumber);

        Task<Shipment> UpdateAsync(int id, UpdateShipmentDto shipment);

        Task<ServiceResponse> DeleteAsync(int id);

        Task<string> MarkShipmentAsDelivered(int id);

        Task<string> MarkShipmentAsCanceled(int id);
    }
}
