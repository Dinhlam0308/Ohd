namespace Ohd.Entities
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public int? parent_id { get; set; }
        public DateTime created_at { get; set; }
    }
}