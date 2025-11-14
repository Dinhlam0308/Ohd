namespace Ohd.DTOs.Maintenance
{
    public class MaintenanceWindowCreateDto
    {
        public int? FacilityId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
    }

    public class MaintenanceWindowUpdateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
    }
}