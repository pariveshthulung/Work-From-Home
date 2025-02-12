namespace Clean.Domain.Entities;

public class Address
{
    public string Street { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string PostalCode { get; private set; } = default!;

    private Address() { }

    private Address(string street, string city, string postalCode)
    {
        Street = ValidationGuard.ValidateString(street, nameof(street));
        City = ValidationGuard.ValidateString(city, nameof(city));
        PostalCode = ValidationGuard.ValidateString(postalCode, nameof(postalCode));
    }

    public static Address Create(string street, string city, string postalCode)
    {
        return new Address(street, city, postalCode);
    }

    public void Update(string street, string city, string postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }
}
