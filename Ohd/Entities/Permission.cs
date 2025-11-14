namespace Ohd.Entities
{
    public class Permission
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string? description { get; set; }
    }
}