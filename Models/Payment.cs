using Logex.API.Constants;

namespace Logex.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? CardNumber { get; set; }
        public string? CVC { get; set; }
        public string Status { get; set; } = PaymentStatus.Pending;
        public int? ShipmentId { get; set; }
        public int UserrId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Shipment? Shipment { get; set; }
        public User? User { get; set; }
    }
}
