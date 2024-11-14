using Clean.Application.Dto.Approval;
using Clean.Application.Dto.Base;

namespace Clean.Application.Dto.Request;

public record RequestDto : BaseDto
{
    public int Id { get; init; }
    public Guid EmployeeGuidId { get; init; }
    public int RequestedBy { get; init; }
    public string RequestedByEmail { get; init; } = default!;
    public int RequestedTo { get; init; }
    public string RequestedToEmail { get; init; } = default!;
    public int RequestedTypeId { get; init; }
    public string RequestedType { get; init; } = default!;
    public string Description { get; init; } = default!;
    public ApprovalDto Approval { get; init; } = default!;
    public DateTime ToDate { get; init; }
    public DateTime FromDate { get; init; }
}
