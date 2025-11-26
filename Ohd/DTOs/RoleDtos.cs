namespace Ohd.DTOs.Roles
{
    public class RoleCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class RoleUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class RoleViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}