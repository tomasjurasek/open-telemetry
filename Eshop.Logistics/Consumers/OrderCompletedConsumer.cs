using Eshop.Contracts.Logistics;
using Eshop.Contracts.Order;
using MassTransit;

namespace Eshop.Logistics.Consumers
{
    public class OrderCompletedConsumer : IConsumer<OrderCompletedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            // Delivery Process
            await Task.Delay(TimeSpan.FromSeconds(1));

            await context.Publish(new OrderDeliveredEvent(context.Message.OrderId));
        }
    }
}
