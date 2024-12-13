using MediatR;

namespace Eshop.Basket.Handlers;

public interface IBasketCommand : IRequest
{
    Guid BasketId { get; init; }
}
