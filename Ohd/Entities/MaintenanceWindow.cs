namespace Ohd.Entities
{
    public class MaintenanceWindow
    {
        public long id { get; set; }
        public int? facility_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public string? reason { get; set; }
        public DateTime created_at { get; set; }
    }
}