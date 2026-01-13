namespace Prueba.Application.Orders;

public sealed record GeoPointDto(double Latitude, double Longitude);

public sealed record OrderDto(
    Guid Id,
    string Customer,
    string Product,
    int Quantity,
    GeoPointDto Origin,
    GeoPointDto Destination,
    double DistanceKm,
    double CostUsd
);
