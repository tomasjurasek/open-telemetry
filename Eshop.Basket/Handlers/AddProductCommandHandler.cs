using Eshop.Basket.Store;
using Eshop.ServiceDefaults;
using MediatR;
using OpenTelemetry;
using System.Diagnostics;

namespace Eshop.Basket.Handlers
{
    public class AddProductCommandHandler(IBasketStore store) : IRequestHandler<AddProductCommand>
    {
        public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity("AddProductCommandHandler");

            var basket = await store.GetBasketAsync(request.BasketId);
            if (basket == null)
            {
                basket = new Domain.Basket(request.BasketId,Guid.NewGuid());
                activity?.AddEvent(new ActivityEvent("BasketCreated"));
                BasketMetrics.BasketCreated();
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
