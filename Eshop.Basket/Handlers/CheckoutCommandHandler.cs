using Eshop.Basket.Domain;
using Eshop.Basket.Store;
using Eshop.Contracts;
using Eshop.Contracts.Basket;
using Eshop.ServiceDefaults;
using MassTransit;
using MediatR;

namespace Eshop.Basket.Handlers
{
    public class CheckoutCommandHandler(IBus bus, IBasketStore store) : IRequestHandler<CheckoutCommand>
    {
        public async Task Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            using var activity = ActivityProvider.ActivitySource.StartActivity("CheckoutCommandHandler");

            var basket = await store.GetBasketAsync(request.BasketId);
            basket.AddDeliveryAddress(request.DeliveryAddress.Map());

            await store.StoreAsync(basket);
            await bus.Publish(new OrderCheckedEvent(request.BasketId/*request.DeliveryAddress, basket.Items.Map())*/));
        }
    }



    public class CheckoutCommand : BasketCommand
    {
        public required Contracts.Address DeliveryAddress { get; init; }
    }
}
