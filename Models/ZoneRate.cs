namespace Logex.API.Models
{
    // Table: ZoneRates (The Dynamic Pricing Matrix)
    // Defines the cost from one Zone to another
    public class ZoneRate
    {
        public int Id { get; set; }

        // TODO: Add uniqe constraint on (FromZoneId, ToZoneId)
        public int FromZoneId { get; set; }
        public int ToZoneId { get; set; }

        // The base cost for moving a shipment between these zones
        public decimal BaseRate { get; set; }

        // Optional: Cost per extra KG for this specific route
        public decimal? AdditionalWeightCost { get; set; }
    }
}
