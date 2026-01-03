namespace Logex.API.Dtos.ShipmentMethodDtos
{
    public record CreateShipmentMethodDto(
        string Name,
        decimal Cost,
        string Duration,
        string? Description,
        bool IsActive
    );
}
