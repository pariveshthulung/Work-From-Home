namespace Clean.Domain.Entities;

public class ApprovalStatus
{
    public int Id { get; private set; }
    public string Status { get; private set; }

    private ApprovalStatus(int id, string status)
    {
        Status = status;
        Id = id;
    }

    public static ApprovalStatus Create(int id, string status)
    {
        return new ApprovalStatus(id, status);
    }

    public void Update(string status)
    {
        Status = status;
    }
}
