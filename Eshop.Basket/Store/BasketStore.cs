using Eshop.ServiceDefaults;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Eshop.Basket.Store
{
    public class BasketStore : IBasketStore
    {
        private readonly IDistributedCache store;

        public BasketStore(IDistributedCache store)
        {
            this.store = store;
        }

        public async Task<Domain.Basket> GetBasketAsync(Guid basketId)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity(nameof(GetBasketAsync));
            activity?.SetTag("basketId", basketId.ToString());

            var json = await store.GetStringAsync(basketId.ToString());

            if (json == null)
            {
                activity?.AddEvent(new System.Diagnostics.ActivityEvent("BasketNotFound"));
            }

            return JsonSerializer.Deserialize<Domain.Basket>(json);

        }

        public async Task StoreAsync(Domain.Basket basket)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity(nameof(GetBasketAsync));
            activity?.SetTag("basketId", basket.Id.ToString());

            var json = JsonSerializer.Serialize(basket);
            await store.SetStringAsync(basket.Id.ToString(), json);
        }
    }
}
