namespace Clean.Application.Dto.Approval;

public record class ApprovalDto
{
    public int ApprovalStatusId { get; init; }
    public string ApprovalStatus { get; init; }
    public int RequestId { get; init; }
    public int ApproverId { get; init; }
    public string ApproverEmail { get; init; }
}
