namespace Ohd.Entities
{
    public class SystemSetting
    {
        public string key { get; set; } = null!;
        public string? value_json { get; set; }
        public DateTime updated_at { get; set; }
    }
}