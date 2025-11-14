namespace Ohd.Entities
{
    public class Facility
    {
        public int Id { get; set; }   // PascalCase
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long? HeadUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}