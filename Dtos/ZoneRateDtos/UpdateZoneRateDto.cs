namespace Logex.API.Dtos.ZoneRateDtos
{
    public record class UpdateZoneRateDto(decimal BaseRate, decimal? AdditionalWeightCost);
}
