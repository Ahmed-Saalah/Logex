using Logex.API.Common;
using Logex.API.Constants;
using Logex.API.Dtos.ShipmentDtos;
using Logex.API.Helpers;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IPricingService _pricingService;

        public ShipmentService(
            IShipmentRepository shipmentRepository,
            IShipmentMethodRepository shipmentMethodRepository,
            IPricingService pricingService
        )
        {
            _shipmentRepository = shipmentRepository;
            _pricingService = pricingService;
        }

        public async Task<Shipment> GetByIdAsync(int id)
        {
            return await _shipmentRepository.GetByIdAsync(id);
        }

        public async Task<Shipment> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await _shipmentRepository.GetShipmentByTrackingNumberAsync(trackingNumber);
        }

        public async Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto, int userId)
        {
            decimal calculatedCost = await _pricingService.CalculateShipmentTotalAsync(
                dto.ShipperCityId,
                dto.ReceiverCityId,
                dto.Quantity,
                dto.Weight,
                dto.ShipmentMethodId
            );

            string trackingNumber = WaybillNumberGenerator.Generate();

            try
            {
                var shipment = new Shipment
                {
                    TrackingNumber = trackingNumber,
                    CreatedAt = DateTime.UtcNow,
                    Quantity = dto.Quantity,
                    Weight = dto.Weight,
                    Description = dto.Description!,
                    Status = ShipmentStatus.Pending,
                    TotalCost = calculatedCost,
                    ShipperName = dto.ShipperName,
                    ShipperEmail = dto.ShipperEmail!,
                    ShipperPhone = dto.ShipperPhone,
                    ShipperStreet = dto.ShipperStreet,
                    ShipperCityId = dto.ShipperCityId,
                    ReceiverName = dto.ReceiverName,
                    ReceiverEmail = dto.ReceiverEmail!,
                    ReceiverPhone = dto.ReceiverPhone,
                    ReceiverStreet = dto.ReceiverStreet,
                    ReceiverCityId = dto.ReceiverCityId,
                    ShipmentMethodId = dto.ShipmentMethodId,
                    UserId = userId,
                };

                var createdShipment = await _shipmentRepository.AddAsync(shipment);
                return createdShipment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Shipment> UpdateAsync(int id, UpdateShipmentDto dto)
        {
            var existingShipment = await _shipmentRepository.GetByIdAsync(id);

            if (existingShipment == null)
            {
                throw new KeyNotFoundException($"Shipment with ID {id} not found.");
            }

            if (existingShipment.Status != ShipmentStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Cannot update a shipment that is already in progress."
                );
            }

            bool priceAffectingChange =
                existingShipment.Weight != dto.Weight
                || existingShipment.Quantity != dto.Quantity
                || existingShipment.ShipperCityId != dto.ShipperCityId
                || existingShipment.ReceiverCityId != dto.ReceiverCityId
                || existingShipment.ShipmentMethodId != dto.ShipmentMethodId;

            if (priceAffectingChange)
            {
                existingShipment.TotalCost = await _pricingService.CalculateShipmentTotalAsync(
                    dto.ShipperCityId,
                    dto.ReceiverCityId,
                    dto.Quantity,
                    dto.Weight,
                    dto.ShipmentMethodId
                );
            }

            existingShipment.ShipperCityId = dto.ShipperCityId;
            existingShipment.ShipperStreet = dto.ShipperStreet;
            existingShipment.ShipperPhone = dto.ShipperPhone;
            existingShipment.ReceiverCityId = dto.ReceiverCityId;
            existingShipment.ReceiverStreet = dto.ReceiverStreet;
            existingShipment.ReceiverPhone = dto.ReceiverPhone;
            existingShipment.ShipmentMethodId = dto.ShipmentMethodId;
            existingShipment.Quantity = dto.Quantity;
            existingShipment.Weight = dto.Weight;

            await _shipmentRepository.UpdateAsync(existingShipment);

            return existingShipment;
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

        public async Task<string> MarkShipmentAsDelivered(int id)
        {
            var shipemnt = await _shipmentRepository.GetByIdAsync(id);
            if (shipemnt == null)
            {
                throw new KeyNotFoundException("Shipment not found.");
            }
            shipemnt.Status = ShipmentStatus.Delivered;

            await _shipmentRepository.UpdateAsync(shipemnt);
            return shipemnt.Status;
        }

        public async Task<string> MarkShipmentAsCanceled(int id)
        {
            var shipemnt = await _shipmentRepository.GetByIdAsync(id);
            if (shipemnt == null)
            {
                throw new KeyNotFoundException("Shipment not found.");
            }
            shipemnt.Status = ShipmentStatus.Cancelled;

            await _shipmentRepository.UpdateAsync(shipemnt);
            return shipemnt.Status;
        }
    }
}
