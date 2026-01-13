using Prueba.Domain.Entities;
using Prueba.Domain.Interfaces;
using Prueba.Domain.Services;
using Prueba.Domain.ValueObjects;

namespace Prueba.Application.Orders.CreateOrder;

public sealed class CreateOrderService
{
    private readonly IDistanceCalculator _distanceCalculator;
    private readonly ICostCalculator _costCalculator;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderService(
        IDistanceCalculator distanceCalculator,
        ICostCalculator costCalculator,
        IOrderRepository orderRepository)
    {
        _distanceCalculator = distanceCalculator;
        _costCalculator = costCalculator;
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResult> ExecuteAsync(CreateOrderCommand cmd, CancellationToken ct = default)
    {
        var origin = new GeoPoint(cmd.OriginLat, cmd.OriginLon);
        var destination = new GeoPoint(cmd.DestinationLat, cmd.DestinationLon);

        var distanceKm = _distanceCalculator.CalculateKm(origin, destination);

        DistanceRules.EnsureValid(distanceKm);

        var costUsd = _costCalculator.CalculateUsd(distanceKm);

        var order = new Order(
            cmd.Customer,
            cmd.Product,
            cmd.Quantity,
            origin,
            destination,
            distanceKm,
            costUsd
        );

        await _orderRepository.AddAsync(order, ct);

        return new CreateOrderResult(
            order.Id,
            order.Customer,
            order.Product,
            order.Quantity,
            order.Origin.Latitude,
            order.Origin.Longitude,
            order.Destination.Latitude,
            order.Destination.Longitude,
            order.DistanceKm,
            order.CostUsd
        );
    }
}
