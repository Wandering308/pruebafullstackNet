using MediatR;
using Prueba.Domain.Interfaces;

namespace Prueba.Application.Orders.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerHandler
    : IRequestHandler<GetOrdersByCustomerQuery, IReadOnlyList<Prueba.Application.Orders.OrderDto>>
{
    private readonly IOrderRepository _repo;

    public GetOrdersByCustomerHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<Prueba.Application.Orders.OrderDto>> Handle(
        GetOrdersByCustomerQuery request,
        CancellationToken ct)
    {
        var orders = await _repo.GetByCustomerAsync(request.Customer, ct);

        return orders.Select(o => new Prueba.Application.Orders.OrderDto(
            o.Id,
            o.Customer,
            o.Product,
            o.Quantity,
            new Prueba.Application.Orders.GeoPointDto(o.Origin.Latitude, o.Origin.Longitude),
            new Prueba.Application.Orders.GeoPointDto(o.Destination.Latitude, o.Destination.Longitude),
            o.DistanceKm,
            o.CostUsd
        )).ToList();
    }
}
