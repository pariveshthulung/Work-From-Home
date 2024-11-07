using System;

namespace Clean.Application.Dto.Approval;

public class ApproveRequestDto
{
    public Guid EmployeeId { get; init; }
    public int ApprovalStatusId { get; init; }
    public Guid RequestId { get; init; }
}
