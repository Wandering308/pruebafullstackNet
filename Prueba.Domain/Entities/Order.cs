using Prueba.Domain.ValueObjects;

namespace Prueba.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Customer { get; private set; } = default!;
    public string Product { get; private set; } = default!;
    public int Quantity { get; private set; }

    public GeoPoint Origin { get; private set; } = default!;
    public GeoPoint Destination { get; private set; } = default!;

    public double DistanceKm { get; private set; }
    public double CostUsd { get; private set; }

    private Order() { } // EF Core

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
    {
        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Customer is required", nameof(customer));

        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product is required", nameof(product));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than 0.");

        return new Order(
            customer.Trim(),
            product.Trim(),
            quantity,
            origin,
            destination,
            distanceKm,
            costUsd
        );
    }
}
