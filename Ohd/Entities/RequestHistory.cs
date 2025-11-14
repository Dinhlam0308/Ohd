using System;

namespace Ohd.Entities
{
    public class RequestHistory
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public int? FromStatusId { get; set; }
        public int ToStatusId { get; set; }
        public long ChangedByUserId { get; set; }
        public string? Remarks { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}