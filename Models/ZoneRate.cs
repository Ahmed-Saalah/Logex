namespace Logex.API.Models
{
    // Table: ZoneRates (The Dynamic Pricing Matrix)
    // Defines the cost from one Zone to another
    public class ZoneRate
    {
        public int Id { get; set; }

        public int FromZoneId { get; set; }
        public int ToZoneId { get; set; }
        public Zone FromZone { get; set; }
        public Zone ToZone { get; set; }

        // The base cost for moving a shipment between these zones
        public decimal BaseRate { get; set; }

        // Optional: Cost per extra KG for this specific route
        public decimal? AdditionalWeightCost { get; set; }
    }
}
