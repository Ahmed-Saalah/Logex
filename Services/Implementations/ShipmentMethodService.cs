using FluentValidation;
using Logex.API.Dtos.ShipmentMethodDtos;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class ShipmentMethodService : IShipmentMethodService
    {
        private readonly IShipmentMethodRepository _shipmentMethodRepository;
        private IValidator<CreateShipmentMethodDto> _validator;

        public ShipmentMethodService(
            IShipmentMethodRepository shipmentMethodRepository,
            IValidator<CreateShipmentMethodDto> validator
        )
        {
            _shipmentMethodRepository = shipmentMethodRepository;
            _validator = validator;
        }

        public async Task<ShipmentMethod> GetByIdAsync(int id)
        {
            return await _shipmentMethodRepository.GetByIdAsync(id);
        }

        public async Task<decimal> GetShipmentMethodCostAsync(int id)
        {
            var shipmentMethod = await _shipmentMethodRepository.GetByIdAsync(id);

            if (shipmentMethod == null)
            {
                throw new Exception("Shipment method not found.");
            }

            return shipmentMethod.Cost;
        }

        public async Task<IEnumerable<ShipmentMethod>> GetAllAsync()
        {
            return await _shipmentMethodRepository.GetAllAsync();
        }

        public async Task<ShipmentMethod> CreateMethodAsync(CreateShipmentMethodDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var exists = await _shipmentMethodRepository.ExistsAsync(_ => _.Name == request.Name);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Shipment method '{request.Name}' already exists."
                );
            }

            var newMethod = new ShipmentMethod
            {
                Name = request.Name,
                Cost = request.Cost,
                Description = request.Description,
                Duration = request.Duration,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _shipmentMethodRepository.AddAsync(newMethod);
            return newMethod;
        }

        public async Task<ShipmentMethod> UpdateMethodAsync(int id, UpdateShipmentMethodDto request)
        {
            var method = await _shipmentMethodRepository.GetByIdAsync(id);
            if (method == null)
            {
                throw new KeyNotFoundException($"Shipment Method ID {id} not found.");
            }

            method.Name = request.Name;
            method.Cost = request.Cost;
            method.Duration = request.Duration;
            method.Description = request.Description;

            await _shipmentMethodRepository.UpdateAsync(method);
            return method;
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var method = await _shipmentMethodRepository.GetByIdAsync(id);
            if (method == null)
            {
                throw new KeyNotFoundException("Method not found.");
            }

            method.IsActive = !method.IsActive;

            await _shipmentMethodRepository.UpdateAsync(method);
            return method.IsActive;
        }
    }
}
