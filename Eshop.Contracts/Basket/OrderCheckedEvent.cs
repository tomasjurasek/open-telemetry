namespace Eshop.Contracts.Basket;

public record OrderCheckedEvent(Guid BasketId, Address DeliveryAddress, IList<Item> Items);
