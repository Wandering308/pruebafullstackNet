using MediatR;
using Prueba.Application.Orders;

namespace Prueba.Application.Orders.GetOrdersByCustomer;

public sealed record GetOrdersByCustomerQuery(string Customer)
    : IRequest<IReadOnlyList<OrderDto>>;
