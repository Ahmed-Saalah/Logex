using Logex.API.Constants;

namespace Logex.API.Dtos.ShipmentDtos
{
    public class CreateShipmentDto
    {
        // Shipper
        public string ShipperName { get; set; }
        public string? ShipperEmail { get; set; } = string.Empty;
        public string ShipperPhone { get; set; }
        public string ShipperStreet { get; set; }
        public int ShipperCityId { get; set; }

        // Receiver
        public string ReceiverName { get; set; }
        public string? ReceiverEmail { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; }
        public string ReceiverStreet { get; set; }
        public int ReceiverCityId { get; set; }

        // Details
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public int ShipmentMethodId { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
