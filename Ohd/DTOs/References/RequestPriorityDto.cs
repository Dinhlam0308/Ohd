namespace Ohd.DTOs.References
{
    public class RequestPriorityCreateDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; } = 0;
    }

    public class RequestPriorityUpdateDto
    {
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}