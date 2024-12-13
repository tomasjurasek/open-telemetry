namespace Eshop.Basket.Domain;

public record Item
{
    public Item(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}


public static class ItemMapper
{
    public static IList<Contracts.Item> Map(this IList<Item> items) => items.Select(items => new Contracts.Item(items.ProductId, items.Quantity)).ToList();
}
