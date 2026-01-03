namespace Logex.API.Models
{
    public class ShipmentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string? Duration { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public ICollection<Shipment>? Shipments { get; set; }
    }
}
