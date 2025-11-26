using System.ComponentModel.DataAnnotations.Schema;

namespace Ohd.Entities
{
    public class RequestStatus
    {
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("is_final")]
        public bool IsFinal { get; set; }

        [Column("is_overdue")]
        public bool IsOverdue { get; set; }

        [Column("color")]
        public string? Color { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}