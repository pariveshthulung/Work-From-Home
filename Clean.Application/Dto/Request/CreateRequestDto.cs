namespace Clean.Application.Dto.Request;

public record CreateRequestDto
{
    public string RequestedToEmail { get; set; }
    public int RequestedTo { get; set; }
    public DateTime ToDate { get; init; }
    public DateTime FromDate { get; init; }
}
