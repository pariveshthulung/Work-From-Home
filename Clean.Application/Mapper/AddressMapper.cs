using Clean.Application.Dto.Address;
using Clean.Domain.Entities;

namespace Clean.Application.Mapper;

public static class AddressMapper
{
    public static Address ToAddress(this AddressDto address)
    {
        return Address.Create(address.Street, address.City, address.PostalCode);
    }

    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            PostalCode = address.PostalCode,
        };
    }
}
