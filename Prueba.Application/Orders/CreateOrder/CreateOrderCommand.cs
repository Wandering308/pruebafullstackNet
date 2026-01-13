using MediatR;

namespace Prueba.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(
    string Customer,
    string Product,
    int Quantity,
    double OriginLat,
    double OriginLon,
    double DestinationLat,
    double DestinationLon
) : IRequest<CreateOrderResult>;
