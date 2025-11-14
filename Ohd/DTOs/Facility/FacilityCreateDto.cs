namespace Ohd.DTOs.Facility
{
    public class FacilityCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? HeadUserId { get; set; }
    }
    public class FacilityUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? HeadUserId { get; set; }
    }

}