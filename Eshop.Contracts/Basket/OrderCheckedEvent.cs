namespace Eshop.Contracts.Basket;

public record OrderCheckedEvent(Address DeliveryAddress, IReadOnlyCollection<Item> Items);
