namespace Clean.Domain.Enums;

public class ApprovalStatusEnum : Enumeration
{
    private ApprovalStatusEnum(int id, string status)
        : base(id, status) { }

    public static readonly ApprovalStatusEnum Draft = new ApprovalStatusEnum(1, "Draft");
    public static readonly ApprovalStatusEnum Pending = new ApprovalStatusEnum(2, "Pending");
    public static readonly ApprovalStatusEnum Accepted = new ApprovalStatusEnum(3, "Accepted");
    public static readonly ApprovalStatusEnum Rejected = new ApprovalStatusEnum(4, "Rejected");

    private static readonly List<ApprovalStatusEnum> _statuses = new List<ApprovalStatusEnum>
    {
        Draft,
        Pending,
        Accepted,
        Rejected
    };
    public static IEnumerable<ApprovalStatusEnum> Statuses => _statuses;

    public static ApprovalStatusEnum FromId(int id)
    {
        return Statuses.FirstOrDefault(s => s.Id == id)
            ?? throw new ArgumentException($"No status with id {id} is found.");
    }

    public static ApprovalStatusEnum FromName(string status)
    {
        return Statuses.FirstOrDefault(s => s.Name == status)
            ?? throw new ArgumentException($"No Status with {status} is found.");
    }
}
