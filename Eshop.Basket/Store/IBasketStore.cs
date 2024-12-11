namespace Eshop.Basket.Store
{
    public interface IBasketStore
    {
        Task<Domain.Basket> GetBasketAsync(Guid basketId);
        Task StoreAsync(Domain.Basket basket);
    }
}