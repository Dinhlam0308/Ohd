using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ohd.Entities
{
    [Table("requests")]
    public class Request
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("requestor_id")]
        public long RequestorId { get; set; }

        [Column("facility_id")]
        public int FacilityId { get; set; }

        [Column("assignee_id")]
        public long? AssigneeId { get; set; }

        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("requester_email")]
        public string? RequesterEmail { get; set; }

        // FOREIGN KEY SeverityId
        [Column("severity_id")]
        public int SeverityId { get; set; }

        [ForeignKey(nameof(SeverityId))]
        public Severity? Severity { get; set; }


        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("priority_id")]
        public int? PriorityId { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [Column("first_reminder_sent")]
        public bool FirstReminderSent { get; set; }

        [Column("last_reminder_at")]
        public DateTime? LastReminderAt { get; set; }
    }
}