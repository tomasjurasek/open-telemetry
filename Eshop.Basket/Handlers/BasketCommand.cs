namespace Eshop.Basket.Handlers
{
    public abstract class BasketCommand : IBasketCommand
    {
        public required Guid BasketId { get; init; }
    }
}
