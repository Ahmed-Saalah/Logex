namespace Logex.API.Dtos.ShipmentDtos
{
    public class UpdateShipmentDto
    {
        public int ShipperCityId { get; set; }
        public string ShipperStreet { get; set; }
        public string ShipperPhone { get; set; }

        public int ReceiverCityId { get; set; }
        public string ReceiverStreet { get; set; }
        public string ReceiverPhone { get; set; }

        public int ShipmentMethodId { get; set; }
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
    }
}
