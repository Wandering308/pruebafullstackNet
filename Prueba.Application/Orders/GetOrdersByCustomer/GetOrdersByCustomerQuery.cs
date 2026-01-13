using MediatR;

namespace Prueba.Application.Orders.GetOrdersByCustomer;

public sealed record GetOrdersByCustomerQuery(string Customer)
    : IRequest<IReadOnlyList<Prueba.Application.Orders.OrderDto>>;
