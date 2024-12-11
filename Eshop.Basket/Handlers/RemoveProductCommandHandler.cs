using Eshop.Basket.Store;
using Eshop.ServiceDefaults;
using MediatR;
using OpenTelemetry;

namespace Eshop.Basket.Handlers
{
    public class RemoveProductCommandHandler(IBasketStore store) : IRequestHandler<RemoveProductCommand>
    {
        public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            // Move to middleware
            Baggage.SetBaggage("basketId", request.BasketId.ToString());

            using var activity = ActivityProvider.ActivitySource.StartActivity("RemoveProductCommandHandler");

            var basket = await store.GetBasketAsync(request.BasketId);
            if (basket == null)
            {
                // TODO Error
            }

            basket.RemoveProduct(request.ProductId);

            await store.StoreAsync(basket);
        }
    }


    public class RemoveProductCommand : BasketCommand
    {
        public Guid ProductId { get; init; }
    }
}
