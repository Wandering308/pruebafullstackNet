using Prueba.Domain.Entities;

namespace Prueba.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);

    Task<IReadOnlyList<Order>> GetByCustomerAsync(string customer, CancellationToken ct = default);

    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default);
}
