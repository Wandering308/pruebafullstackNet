namespace Prueba.Application.Orders;

public sealed record OrderDto(
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
