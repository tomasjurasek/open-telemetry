namespace Eshop.Basket.Domain;

public record Address(string FirstName, string LastName, string City, string Street);


public static class AddressMapper
{
    public static Address Map(this Contracts.Address address) => new Address(address.FirstName, address.LastName, address.City, address.Street);
}


