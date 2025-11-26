namespace Ohd.DTOs.SystemSettings
{
    public class SystemSettingDto
    {
        public string Key { get; set; } = null!;
        public string? Value { get; set; } 
        public string? Description { get; set; }
    }
}