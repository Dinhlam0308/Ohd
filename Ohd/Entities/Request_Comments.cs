using System;

namespace Ohd.Entities
{
    public class Request_Comments
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public long AuthorUserId { get; set; }
        public string Body { get; set; } = null!;
        public int AttachmentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}