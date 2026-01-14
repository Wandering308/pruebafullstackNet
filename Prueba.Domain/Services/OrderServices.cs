using Prueba.Domain.Entities;
using Prueba.Domain.Interfaces;
using Prueba.Domain.ValueObjects;

namespace Prueba.Domain.Services;

public sealed class OrderServices
{
    private readonly IOrderRepository _repo;
    private readonly IDistanceCalculator _distance;
    private readonly ICostCalculator _cost;

    public OrderServices(IOrderRepository repo, IDistanceCalculator distance, ICostCalculator cost)
    {
        _repo = repo;
        _distance = distance;
        _cost = cost;
    }

    public async Task<Order> CreateOrderAsync(
        string customer,
        string product,
        int quantity,
        double originLat,
        double originLon,
        double destinationLat,
        double destinationLon,
        CancellationToken ct = default)
    {
        var origin = new GeoPoint(originLat, originLon);
        var destination = new GeoPoint(destinationLat, destinationLon);

        var distanceKm = _distance.CalculateKm(origin, destination);
        DistanceRules.EnsureValid(distanceKm);

        var costUsd = _cost.CalculateUsd(distanceKm);

        // ✅ IMPORTANTE: usa el factory si existe; si no, usamos constructor
        // Si ya tienes: Order.Create(...) úsalo:
        // var order = Order.Create(customer, product, quantity, origin, destination, distanceKm, costUsd);

        var order = Order.Create(
            customer,
            product,
            quantity,
            origin,
            destination,
            distanceKm,
            costUsd
        );



        await _repo.AddAsync(order, ct);
        return order;
    }
}
