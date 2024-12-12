using Eshop.ServiceDefaults;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Eshop.Basket.Store
{
    public class BasketStore : IBasketStore
    {
        private readonly IConnectionMultiplexer store;

        public BasketStore(IConnectionMultiplexer store)
        {
            this.store = store;
        }

        public async Task<Domain.Basket?> GetBasketAsync(Guid basketId)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity(nameof(GetBasketAsync));
            var db = store.GetDatabase();
            var json = await db.StringGetAsync(basketId.ToString());

            if (!json.HasValue)
            {
                activity?.AddEvent(new System.Diagnostics.ActivityEvent("BasketNotFound"));
                return null;
            }

            return JsonSerializer.Deserialize<Domain.Basket>(json);

        }

        public async Task StoreAsync(Domain.Basket basket)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity(nameof(StoreAsync));
            var db = store.GetDatabase();

            var json = JsonSerializer.Serialize(basket);
            await db.StringSetAsync(basket.Id.ToString(), json);
        }
    }
}
