namespace Ohd.DTOs.Severity
{
    // Dùng khi tạo mới
    public class SeverityCreateDto
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool AutoNotify { get; set; } = false;
        public string? NotifyEmailTemplateCode { get; set; }
    }

    public class SeverityUpdateDto
    {
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
        public bool AutoNotify { get; set; } = false;
        public string? NotifyEmailTemplateCode { get; set; }
    }


    // Dùng để trả về cho FE
    public class SeverityViewDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}