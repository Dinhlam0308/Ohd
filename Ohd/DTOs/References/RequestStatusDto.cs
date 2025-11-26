namespace Ohd.DTOs.References
{
    // Dùng khi tạo mới
    public class RequestStatusCreateDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsFinal { get; set; }
        public bool IsOverdue { get; set; } = false; // ✅ mới
        public string? Color { get; set; } // ✅ mới, vd "#ef4444"
    }

    // Dùng khi update: KHÔNG cho đổi Code
    public class RequestStatusUpdateDto
    {
        public string Name { get; set; } = null!;
        public bool IsFinal { get; set; }
        public bool IsOverdue { get; set; } = false;
        public string? Color { get; set; }
    }

    public class RequestStatusViewDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsFinal { get; set; }
        public bool IsOverdue { get; set; }
        public string? Color { get; set; }
    }
}
