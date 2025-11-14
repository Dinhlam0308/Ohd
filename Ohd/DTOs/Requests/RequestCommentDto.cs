namespace Ohd.DTOs.Requests
{
    public class RequestCommentCreateDto
    {
        public long RequestId { get; set; }
        public long AuthorUserId { get; set; }
        public string Body { get; set; } = null!;
    }

    public class RequestCommentUpdateDto
    {
        public string Body { get; set; } = null!;
    }
}
