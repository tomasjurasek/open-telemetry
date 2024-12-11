using Eshop.Basket.Store;
using Eshop.ServiceDefaults;
using MediatR;
using OpenTelemetry;

namespace Eshop.Basket.Handlers
{
    public class AddProductCommandHandler(IBasketStore store) : IRequestHandler<AddProductCommand>
    {
        public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            // Move to middleware
            Baggage.SetBaggage("basketId", request.BasketId.ToString());

            using var activity = ActivityProvider.ActivitySource.StartActivity("AddProductCommandHandler");

            var basket = await store.GetBasketAsync(request.BasketId);
            if (basket == null)
            {
                // TODO Error
            }

            basket.AddProduct(request.ProductId);

            await store.StoreAsync(basket);
        }
    }


    public class AddProductCommand : BasketCommand
    {
        public Guid ProductId { get; init; }
    }
}
