namespace Clean.Application.Dto.Address;

public record AddressDto
{
    public string Street { get; init; }
    public string City { get; init; }
    public string PostalCode { get; init; }
}
