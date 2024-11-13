using Clean.Application.Dto.Approval;
using Clean.Application.Dto.Base;

namespace Clean.Application.Dto.Request;

public record RequestDto : BaseDto
{
    public int Id { get; init; }
    public Guid EmployeeGuidId { get; init; }
    public int RequestedBy { get; init; }
    public string RequestedByEmail { get; init; }
    public int RequestedTo { get; init; }
    public string RequestedToEmail { get; init; }
    public int RequestedTypeId { get; init; }
    public string RequestedType { get; init; }
    public ApprovalDto Approval { get; init; }
    public DateTime ToDate { get; init; }
    public DateTime FromDate { get; init; }
    public string Description { get; init; }
}
