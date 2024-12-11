using MassTransit;
using Eshop.Contracts.Basket;
using Eshop.Contracts.Payment;
using Eshop.Contracts.Order;
using Eshop.Contracts.Logistics;

namespace Eshop.Order.Saga
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public Event<OrderCheckedEvent> OrderChecked { get; private set; }
        //public Event<OrderPaidEvent> OrderPaid { get; private set; }
        //public Event<OrderDeliveredEvent> OrderDelivered { get; private set; }

        public State Accepted { get; private set; }
        //public State Paid { get; private set; }
        //public State Delivered { get; private set; }

        private void ConfigureCorrelationIds()
        {
            Event(() => OrderChecked, x => x.CorrelateById(c => c.Message.BasketId));
        }

        public OrderStateMachine()
        {
            ConfigureCorrelationIds();

            InstanceState(c => c.State);

            Initially(
                When(OrderChecked)
                    .Then(context => context.Saga.BasketId = context.Message.BasketId)
                    .Then(context => context.Saga.OrderId = Guid.NewGuid())
                    .TransitionTo(Accepted)
            );

            //During(Accepted,
            //    When(OrderPaid)
            //        .Then(context => context.Saga.PaidAt = DateTimeOffset.UtcNow)
            //        .TransitionTo(Paid)
            //        .Publish(context => new OrderCompletedEvent(context.Saga.OrderId))
            //);

            //During(Paid,
            //    When(OrderDelivered)
            //        .Then(context => context.Saga.DeliveredAt = DateTimeOffset.UtcNow)
            //        .TransitionTo(Delivered)
            //);
        }
    }


    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public Guid BasketId { get; set; }
        public Guid OrderId { get; set; }

        public DateTimeOffset? PaidAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public string State { get; set; }
    }
}
