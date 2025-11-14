namespace Ohd.DTOs.Requests
{
    public class AttachmentCreateDto
    {
        public long RequestId { get; set; }
        public long UploadedByUserId { get; set; }
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long FileSizeBytes { get; set; }
        public string StorageUrl { get; set; } = null!;
    }
}