namespace Ohd.DTOs.Sla
{
    public class SlaPolicyCreateDto
    {
        public int? FacilityId { get; set; }
        public int? CategoryId { get; set; }
        public int Priority { get; set; }
        public int RespondWithinMins { get; set; }
        public int ResolveWithinMins { get; set; }
        public bool Active { get; set; } = true;
    }

    public class SlaPolicyUpdateDto
    {
        public int Priority { get; set; }
        public int RespondWithinMins { get; set; }
        public int ResolveWithinMins { get; set; }
        public bool Active { get; set; }
    }
}