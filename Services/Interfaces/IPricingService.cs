namespace Logex.API.Services.Interfaces
{
    public interface IPricingService
    {
        Task<decimal> CalculateShipmentTotalAsync(
            int shipperCityId,
            int receiverCityId,
            int quantity,
            decimal weight,
            int methodId
        );
    }
}
