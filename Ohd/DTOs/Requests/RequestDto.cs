namespace Ohd.DTOs.Requests
{
    public class RequestCreateDto
    {
        public long RequestorId { get; set; }
        public int FacilityId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SeverityId { get; set; }
        public int? PriorityId { get; set; }
    }

    public class RequestUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? PriorityId { get; set; }
        public string? Remarks { get; set; }
    }

    public class RequestChangeStatusDto
    {
        public int ToStatusId { get; set; }
        public long ChangedByUserId { get; set; }
        public string? Remarks { get; set; }
    }
}