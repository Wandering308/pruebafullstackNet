namespace Prueba.Application.Orders.CreateOrder;

public sealed record CreateOrderResult(
    Guid Id,
    string Customer,
    string Product,
    int Quantity,
    double OriginLat,
    double OriginLon,
    double DestinationLat,
    double DestinationLon,
    double DistanceKm,
    double CostUsd
);
