namespace Clean.Domain.Entities.StoreProcedure;

public class GetAllEmployees
{
    public int Id { get; set; }
    public Guid GuidId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int UserRoleId { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
}
