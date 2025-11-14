namespace Ohd.DTOs.References
{
    public class TagCreateDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class TagUpdateDto
    {
        public string Name { get; set; } = null!;
    }
}