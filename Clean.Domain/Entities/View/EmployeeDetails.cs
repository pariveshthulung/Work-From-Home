using System;

namespace Clean.Domain.Entities.View;

public class EmployeeDetails
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int UserRoleId { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Address_Street { get; set; } = default!;
    public string Address_City { get; set; } = default!;
    public string Address_PostalCode { get; set; } = default!;

    public int? RequestId { get; set; }
    public int? RequestedBy { get; set; }
    public int? RequestedTo { get; set; }
    public int? RequestedTypeId { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

    public int? ApprovalId { get; set; }
    public int? ApprovalStatusId { get; set; }
    public int? ApproverId { get; set; }
}
