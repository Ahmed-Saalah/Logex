using FluentValidation;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class PricingService : IPricingService
    {
        private readonly IPricingRepository _pricingRepository;

        public PricingService(IPricingRepository pricingRepository, IValidator<Shipment> validator)
        {
            _pricingRepository = pricingRepository;
        }

        public async Task<decimal> CalculateShipmentTotalAsync(Shipment shipment)
        {
            var sourceZone = await _pricingRepository.GetZoneByCityNameAsync(
                shipment.ShipperCity?.Trim()
            );

            var destZone = await _pricingRepository.GetZoneByCityNameAsync(
                shipment.ReceiverCity?.Trim()
            );

            if (sourceZone == null || destZone == null)
            {
                throw new InvalidOperationException("Invalid source or destination city.");
            }

            var rateRule =
                await _pricingRepository.GetRateByZonesAsync(sourceZone.Id, destZone.Id)
                ?? throw new InvalidOperationException("No pricing configuration found.");

            decimal totalWeight = shipment.Quantity * shipment.Weight;
            decimal weightCost = totalWeight * shipment.ShipmentMethod.Cost;
            decimal zoneCost = rateRule.BaseRate;

            // If the route has a specific extra cost per KG, add it here
            if (rateRule.AdditionalWeightCost > 0)
            {
                weightCost += (totalWeight * rateRule.AdditionalWeightCost.Value);
            }

            return weightCost + zoneCost;
        }
    }
}
