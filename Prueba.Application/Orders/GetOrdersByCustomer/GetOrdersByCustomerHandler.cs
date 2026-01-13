using MediatR;
using Prueba.Application.Orders;
using Prueba.Domain.Interfaces;

namespace Prueba.Application.Orders.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerHandler
    : IRequestHandler<GetOrdersByCustomerQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderRepository _repo;

    public GetOrdersByCustomerHandler(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken ct)
    {
        var customer = (request.Customer ?? "").Trim();

        if (string.IsNullOrWhiteSpace(customer))
            return Array.Empty<OrderDto>();

        var orders = await _repo.GetByCustomerAsync(customer, ct);

        return orders
            .Select(o => new OrderDto(
                o.Id,
                o.Customer,
                o.Product,
                o.Quantity,
                o.Origin.Latitude,
                o.Origin.Longitude,
                o.Destination.Latitude,
                o.Destination.Longitude,
                o.DistanceKm,
                o.CostUsd
            ))
            .ToList();
    }
}
