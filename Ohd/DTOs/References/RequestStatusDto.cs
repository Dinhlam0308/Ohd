namespace Ohd.DTOs.References
{
    public class RequestStatusCreateDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsFinal { get; set; }
    }

    public class RequestStatusUpdateDto
    {
        public string Name { get; set; } = null!;
        public bool IsFinal { get; set; }
    }

}