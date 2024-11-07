namespace Clean.Domain.Entities.View;

public class SqlInjection
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public int UserRoleId { get; set; }
    public int? RequestId { get; set; }
    public int? RequestedBy { get; set; }
    public int? RequestedTo { get; set; }
    public int? RequestedTypeId { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }
}
