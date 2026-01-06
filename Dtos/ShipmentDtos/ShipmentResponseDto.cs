namespace Logex.API.Dtos.ShipmentDtos
{
    public class ShipmentResponseDto
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShipmentMethod { get; set; } = string.Empty;
        public string ShipperName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
    }
}
