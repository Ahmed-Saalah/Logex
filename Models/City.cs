namespace Logex.API.Models
{
    // Table: Cities (e.g., "Cairo", "Giza" -> ZoneId: 1)
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ZoneId { get; set; }
        public bool IsActive { get; set; } = true;
        public Zone Zone { get; set; }
    }
}
