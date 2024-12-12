using Eshop.Basket.Domain;
using Eshop.Basket.Store;
using Eshop.Contracts.Basket;
using Eshop.ServiceDefaults;
using MassTransit;
using MediatR;

namespace Eshop.Basket.Handlers
{
    public class CheckoutCommandHandler(IBus bus, IBasketStore store, LogisticsClient logisticsClient) : IRequestHandler<CheckoutCommand>
    {
        public async Task Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity("CheckoutCommandHandler");

            var basket = await store.GetBasketAsync(request.BasketId);

            foreach (var item in basket.Items)
            {
                if(!await logisticsClient.IsAvailable(item.ProductId))
                {
                    activity?.AddEvent(new System.Diagnostics.ActivityEvent("ProductIsNotAvailable"));
                    throw new Exception("Is not available");
                }

            }

            basket.AddDeliveryAddress(request.DeliveryAddress.Map());

            await store.StoreAsync(basket);
            await bus.Publish(new OrderCheckedEvent(request.BasketId,request.DeliveryAddress, basket.Items.Map()));
        }
    }



    public class CheckoutCommand : BasketCommand
    {
        public required Contracts.Address DeliveryAddress { get; init; }
    }
}
