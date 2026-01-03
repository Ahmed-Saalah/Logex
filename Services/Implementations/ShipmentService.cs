using Logex.API.Common;
using Logex.API.Dtos.ShipmentDtos;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IShipmentMethodRepository _shipmentMethodRepository;

        public ShipmentService(
            IShipmentRepository shipmentRepository,
            IPaymentRepository paymentRepository,
            IShipmentMethodRepository shipmentMethodRepository
        )
        {
            _shipmentRepository = shipmentRepository;
            _shipmentMethodRepository = shipmentMethodRepository;
        }

        public async Task<Shipment> CreateShipmentAsync(
            CreateShipmentDto shipmentCreateDTO,
            int userId
        )
        {
            ShipmentMethod shipmentMethod = null;

            if (shipmentCreateDTO.ShipmentMethodId.HasValue)
            {
                shipmentMethod = await _shipmentMethodRepository.GetAsync(sm =>
                    sm.Id == shipmentCreateDTO.ShipmentMethodId
                );
            }

            if (shipmentMethod == null)
            {
                shipmentMethod = await _shipmentMethodRepository.GetShipmentMethodByNameAsync(
                    "Standard"
                );
            }

            if (shipmentMethod == null)
            {
                throw new Exception("Unable to find a valid ShipmentMethod.");
            }

            if (shipmentMethod == null)
            {
                throw new Exception("Unable to find a valid ShipmentMethod.");
            }

            try
            {
                var shipment = new Shipment
                {
                    ShipperName = shipmentCreateDTO.ShipperName,
                    ShipperEmail = shipmentCreateDTO.ShipperEmail,
                    ShipperPhone = shipmentCreateDTO.ShipperPhone,
                    ShipperCountry = shipmentCreateDTO.ShipperCountry,
                    ShipperCity = shipmentCreateDTO.ShipperCity,
                    ShipperStreet = shipmentCreateDTO.ShipperStreet,
                    ReceiverName = shipmentCreateDTO.ReceiverName,
                    ReceiverEmail = shipmentCreateDTO.ReceiverEmail,
                    ReceiverPhone = shipmentCreateDTO.ReceiverPhone,
                    ReceiverCountry = shipmentCreateDTO.ReceiverCountry,
                    ReceiverCity = shipmentCreateDTO.ReceiverCity,
                    ReceiverStreet = shipmentCreateDTO.ReceiverStreet,
                    Quantity = shipmentCreateDTO.Quantity,
                    Weight = shipmentCreateDTO.Weight,
                    Description = shipmentCreateDTO.Description,
                    Status = shipmentCreateDTO.Status,
                    TrackingNumber = Guid.NewGuid()
                        .ToString()
                        .Replace("-", "")
                        .Substring(0, 10)
                        .ToUpper(),
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ShipmentMethod = shipmentMethod,
                };

                var createdShipment = await _shipmentRepository.AddAsync(shipment);

                return createdShipment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await _shipmentRepository.GetByIdAsync(id);
        }

        public async Task<Shipment> GetByTrackingNumber(string trackingNumber)
        {
            return await _shipmentRepository.GetShipmentByTrackingNumberAsync(trackingNumber);
        }

        public async Task<ServiceResponse> UpdateShipmentAsync(int id, UpdateShipmentDto shipment)
        {
            var existingShipment = await _shipmentRepository.GetByIdAsync(id);

            if (existingShipment == null)
            {
                return new ServiceResponse(false, "Shipment not found.");
            }

            existingShipment.ShipperCountry = shipment.ShipperCountry;
            existingShipment.ShipperCity = shipment.ShipperCity;
            existingShipment.ShipperStreet = shipment.ShipperStreet;
            existingShipment.ShipperPhone = shipment.ShipperPhone;

            existingShipment.ReceiverCountry = shipment.ReceiverCountry;
            existingShipment.ReceiverCity = shipment.ReceiverCity;
            existingShipment.ReceiverStreet = shipment.ReceiverStreet;
            existingShipment.ReceiverPhone = shipment.ReceiverPhone;

            await _shipmentRepository.UpdateAsync(existingShipment);
            return new ServiceResponse(false, "Shipment updated successfully");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(id);
            if (shipment is null)
            {
                return new ServiceResponse(false, "Shipment not found.");
            }

            await _shipmentRepository.DeleteAsync(id);
            return new ServiceResponse(true, "Shipment deleted successfully.");
        }

        public async Task<ServiceResponse> Update(Shipment shipment)
        {
            var existingShipment = await _shipmentRepository.GetByIdAsync(shipment.Id);

            if (existingShipment == null)
            {
                return new ServiceResponse(false, "Shipment not found");
            }

            await _shipmentRepository.UpdateAsync(existingShipment);
            return new ServiceResponse(true, "Shipment updated successfully.");
        }

        public decimal GetTotalCost(int quantity, decimal weight, decimal shipmentMethodCost)
        {
            return weight * quantity + shipmentMethodCost;
        }
    }
}
