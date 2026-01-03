namespace Logex.API.Dtos.ShipmentMethodDtos
{
    public record UpdateShipmentMethodDto(
        string Name,
        decimal Cost,
        string? Duration,
        string? Description,
        bool IsActive
    );
}
