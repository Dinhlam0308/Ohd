namespace Ohd.DTOs.Roles.DepartmentHead;

public class RequestSearchDto
{
    public int? StatusId { get; set; }
    public int? Priority { get; set; }
    public long? AssigneeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; } 
}