namespace Logex.API.Models
{
    // Table: Zones (e.g., "Central", "Delta", "Upper Egypt")
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
