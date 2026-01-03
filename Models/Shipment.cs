using Logex.API.Constants;

namespace Logex.API.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = ShipmentStatus.Pending;
        public decimal TotalCost { get; set; }

        #region shipper Details
        public string ShipperName { get; set; }
        public string ShipperPhone { get; set; }
        public string ShipperEmail { get; set; }
        public string ShipperStreet { get; set; }
        public int ShipperCityId { get; set; }
        public City ShipperCity { get; set; }
        #endregion

        #region Receiver Details
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverStreet { get; set; }
        public int ReceiverCityId { get; set; }
        public City ReceiverCity { get; set; }
        #endregion

        // Relationships
        public int ShipmentMethodId { get; set; }
        public ShipmentMethod ShipmentMethod { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
