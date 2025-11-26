namespace Ohd.DTOs.References
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }

    public class CategoryUpdateDto
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }
    public class CategoryViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
    }

}