namespace Ohd.DTOs.Teams
{
    public class TeamCreateDto
    {
        public string TeamName { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class TeamUpdateDto
    {
        public string TeamName { get; set; } = null!;
        public string? Description { get; set; }
    }
}