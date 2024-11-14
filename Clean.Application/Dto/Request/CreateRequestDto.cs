using Clean.Domain.Enums;

namespace Clean.Application.Dto.Request;

public record CreateRequestDto
{
    public string RequestedToEmail { get; set; } = default!;
    public string Description { get; init; } = default!;
    public int RequestedTypeId { get; set; } = RequestTypeEnum.WorkFromHome.Id;
    public int RequestedTo { get; set; }
    public DateTime ToDate { get; init; }
    public DateTime FromDate { get; init; }
}
