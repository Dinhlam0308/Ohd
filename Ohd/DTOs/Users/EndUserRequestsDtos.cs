namespace Ohd.Dtos.Requests
{
    public class CreateRequestDto
    {
        public int FacilityId { get; set; }
        public int SeverityId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class RequestListItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string FacilityName { get; set; } = null!;
        public string SeverityName { get; set; } = null!;
        public string SeverityCode { get; set; } = null!;
        public string StatusCode { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public string? StatusColor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOverdue { get; set; }
    }

    public class RequestCommentDto
    {
        public long Id { get; set; }
        public long AuthorUserId { get; set; }
        public string Body { get; set; } = null!;
        public int AttachmentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AttachmentDto
    {
        public long Id { get; set; }
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long FileSizeBytes { get; set; }
        public string StorageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class RequestDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = null!;

        public int SeverityId { get; set; }
        public string SeverityCode { get; set; } = null!;
        public string SeverityName { get; set; } = null!;

        public int StatusId { get; set; }
        public string StatusCode { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public string? StatusColor { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? Remarks { get; set; }         // dùng lưu lý do đóng
        public bool IsOverdue { get; set; }

        public List<AttachmentDto> Attachments { get; set; } = new();
        public List<RequestCommentDto> Comments { get; set; } = new();
    }

    public class AddCommentDto
    {
        public string Body { get; set; } = null!;
    }

    public class CloseRequestDto
    {
        public string Reason { get; set; } = null!;
    }
}
