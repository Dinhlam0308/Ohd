using System;

namespace Ohd.Entities
{
    public class Request
    {
        public long Id { get; set; }
        public long RequestorId { get; set; }
        public int FacilityId { get; set; }
        public long? AssigneeId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SeverityId { get; set; }
        public int StatusId { get; set; }
        public int? PriorityId { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}