using Eshop.Contracts;

namespace Eshop.Basket.Domain
{
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
        public static IReadOnlyCollection<Contracts.Item> Map(this IReadOnlyCollection<Item> items) => items.Select(items => new Contracts.Item(items.ProductId, items.Quantity)).ToList();
    }
}
