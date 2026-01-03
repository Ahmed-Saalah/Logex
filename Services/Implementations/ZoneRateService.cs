using Logex.API.Data;
using Logex.API.Dtos.ZoneRateDtos;
using Logex.API.Models;
using Logex.API.Repository.Interfaces;
using Logex.API.Services.Interfaces;

namespace Logex.API.Services.Implementations
{
    public class ZoneRateService : IZoneRateService
    {
        private readonly IZoneRateRepository _zoneRateRepository;

        public ZoneRateService(IZoneRateRepository zoneRateRepository)
        {
            _zoneRateRepository = zoneRateRepository;
        }

        public async Task<IEnumerable<ZoneRate>> GetAllAsync()
        {
            return await _zoneRateRepository.GetAllWithZonesAsync();
        }

        public async Task<ZoneRate> GetByIdAsync(int id)
        {
            var rate = await _zoneRateRepository.GetByIdAsync(id);
            return rate == null
                ? throw new KeyNotFoundException($"Rate rule with ID {id} not found.")
                : rate;
        }

        public async Task<ZoneRate> CreateRateAsync(CreateZoneRateDto request)
        {
            if (await _zoneRateRepository.ExistsRouteAsync(request.FromZoneId, request.ToZoneId))
            {
                throw new InvalidOperationException(
                    "A pricing rule for this route (From -> To) already exists. Please update the existing rule instead."
                );
            }

            var rate = new ZoneRate
            {
                FromZoneId = request.FromZoneId,
                ToZoneId = request.ToZoneId,
                BaseRate = request.BaseRate,
                AdditionalWeightCost = request.AdditionalWeightCost,
            };

            await _zoneRateRepository.AddAsync(rate);
            return rate;
        }

        public async Task<ZoneRate> UpdateRateAsync(int id, UpdateZoneRateDto request)
        {
            var rate =
                await _zoneRateRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Rate rule with ID {id} not found.");

            rate.BaseRate = request.BaseRate;
            rate.AdditionalWeightCost = request.AdditionalWeightCost;

            await _zoneRateRepository.UpdateAsync(rate);
            return rate;
        }

        public async Task DeleteRateAsync(int id)
        {
            var rate =
                await _zoneRateRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Rate rule with ID {id} not found.");
            await _zoneRateRepository.DeleteAsync(rate.Id);
        }
    }
}
