using MediatR;
using Prueba.Application.Orders.CreateOrder;

namespace Prueba.Application.Orders;

public sealed record CreateOrderCommand(
    string Customer,
    string Product,
    int Quantity,
    double OriginLat,
    double OriginLon,
    double DestinationLat,
    double DestinationLon
) : IRequest<CreateOrderResult>;
