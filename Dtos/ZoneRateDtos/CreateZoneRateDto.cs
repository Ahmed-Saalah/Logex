namespace Logex.API.Dtos.ZoneRateDtos
{
    public record CreateZoneRateDto(
        int FromZoneId,
        int ToZoneId,
        decimal BaseRate,
        decimal? AdditionalWeightCost
    );
}
