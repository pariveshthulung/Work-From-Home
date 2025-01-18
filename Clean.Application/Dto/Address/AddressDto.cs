namespace Clean.Application.Dto.Address;

public record AddressDto
{
    public string Street { get; init; } = default!;
    public string City { get; init; } = default!;
    public string PostalCode { get; init; } = default!;
}
