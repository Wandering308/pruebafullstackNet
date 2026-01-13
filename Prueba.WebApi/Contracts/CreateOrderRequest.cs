namespace Prueba.WebApi.Contracts;

public sealed record CreateOrderRequest(
    string Customer,
    string Product,
    int Quantity,
    double OriginLat,
    double OriginLon,
    double DestinationLat,
    double DestinationLon
);
