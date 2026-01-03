using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class PricingService : IPricingService
    {
        private readonly IZoneRateRepository _zoneRateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IShipmentMethodService _shipmentMethodService;

        public PricingService(
            IZoneRateRepository zoneRateRepository,
            ICityRepository cityRepository,
            IShipmentMethodService shipmentMethodService
        )
        {
            _zoneRateRepository = zoneRateRepository;
            _cityRepository = cityRepository;
            _shipmentMethodService = shipmentMethodService;
        }

        public async Task<decimal> CalculateShipmentTotalAsync(
            int shipperCityId,
            int receiverCityId,
            int quantity,
            decimal weight,
            int methodId
        )
        {
            var shipperCity = await _cityRepository.GetByIdAsync(shipperCityId);
            var receiverCity = await _cityRepository.GetByIdAsync(receiverCityId);

            if (shipperCity == null || receiverCity == null)
            {
                throw new InvalidOperationException(
                    "One or both cities not found in coverage area."
                );
            }

            var zoneRate = await _zoneRateRepository.GetByRouteAsync(
                shipperCity.ZoneId,
                receiverCity.ZoneId
            );

            if (zoneRate == null)
            {
                throw new InvalidOperationException(
                    $"No pricing rule defined for route: {shipperCity.Name} -> {receiverCity.Name}"
                );
            }

            var methodBaseCost = await _shipmentMethodService.GetShipmentMethodCostAsync(methodId);

            decimal totalWeight = weight * quantity;

            decimal weightCostPerKg = methodBaseCost + zoneRate.AdditionalWeightCost!.Value;
            decimal total = zoneRate.BaseRate + (totalWeight * weightCostPerKg);

            return total;
        }
    }
}
