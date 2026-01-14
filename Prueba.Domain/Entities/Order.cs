using Prueba.Domain.ValueObjects;

namespace Prueba.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Customer { get; private set; } = default!;
    public string Product { get; private set; } = default!;
    public int Quantity { get; private set; }

    public GeoPoint Origin { get; private set; } = default!;
    public GeoPoint Destination { get; private set; } = default!;

    public double DistanceKm { get; private set; }
    public double CostUsd { get; private set; }

    private Order() { } // EF

    private Order(
        string customer,
        string product,
        int quantity,
        GeoPoint origin,
        GeoPoint destination,
        double distanceKm,
        double costUsd)
    {
        Customer = customer;
        Product = product;
        Quantity = quantity;
        Origin = origin;
        Destination = destination;
        DistanceKm = distanceKm;
        CostUsd = costUsd;
    }

    public static Order Create(
        string customer,
        string product,
        int quantity,
        GeoPoint origin,
        GeoPoint destination,
        double distanceKm,
        double costUsd)
        => new(customer, product, quantity, origin, destination, distanceKm, costUsd);
}
