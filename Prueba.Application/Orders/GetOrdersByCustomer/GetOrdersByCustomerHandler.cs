using AutoMapper;
using MediatR;
using Prueba.Domain.Interfaces;

namespace Prueba.Application.Orders.GetOrdersByCustomer;

public sealed class GetOrdersByCustomerHandler
    : IRequestHandler<GetOrdersByCustomerQuery, IReadOnlyList<Prueba.Application.Orders.OrderDto>>
{
    private readonly IOrderRepository _repo;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerHandler(IOrderRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<Prueba.Application.Orders.OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken ct)
    {
        var customer = (request.Customer ?? "").Trim();
        if (string.IsNullOrWhiteSpace(customer))
            return Array.Empty<Prueba.Application.Orders.OrderDto>();

        var orders = await _repo.GetByCustomerAsync(customer, ct);

        // Mapea lista completa usando AutoMapper
        return orders.Select(o => _mapper.Map<Prueba.Application.Orders.OrderDto>(o)).ToList();
    }
}
