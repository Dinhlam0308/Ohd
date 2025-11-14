namespace Ohd.DTOs.References
{
    public class SeverityCreateDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; } = 0;
    }

    public class SeverityUpdateDto
    {
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}