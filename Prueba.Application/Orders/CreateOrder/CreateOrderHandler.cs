using MediatR;
using Prueba.Domain.Entities;
using Prueba.Domain.Interfaces;
using Prueba.Domain.Services;
using Prueba.Domain.ValueObjects;

namespace Prueba.Application.Orders.CreateOrder;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IOrderRepository _repo;
    private readonly IDistanceCalculator _distance;
    private readonly ICostCalculator _cost;

    public CreateOrderHandler(IOrderRepository repo, IDistanceCalculator distance, ICostCalculator cost)
    {
        _repo = repo;
        _distance = distance;
        _cost = cost;
    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var origin = new GeoPoint(request.OriginLat, request.OriginLon);
        var destination = new GeoPoint(request.DestinationLat, request.DestinationLon);

        var distanceKm = _distance.CalculateKm(origin, destination);
        DistanceRules.EnsureValid(distanceKm);

        var costUsd = _cost.CalculateUsd(distanceKm);

       var order = Order.Create(
            request.Customer,
            request.Product,
            request.Quantity,
            origin,
            destination,
            distanceKm,
            costUsd
        );


        await _repo.AddAsync(order, ct);

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
