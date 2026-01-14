using MediatR;
using Prueba.Domain.Services;

namespace Prueba.Application.Orders.CreateOrder;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly OrderServices _services;

    public CreateOrderHandler(OrderServices services)
    {
        _services = services;
    }

    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = await _services.CreateOrderAsync(
            request.Customer,
            request.Product,
            request.Quantity,
            request.OriginLat,
            request.OriginLon,
            request.DestinationLat,
            request.DestinationLon,
            ct
        );

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
