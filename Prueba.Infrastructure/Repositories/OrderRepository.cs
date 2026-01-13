using Microsoft.EntityFrameworkCore;
using Prueba.Domain.Entities;
using Prueba.Domain.Interfaces;
using Prueba.Infrastructure.Persistence;

namespace Prueba.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _db.Orders.AddAsync(order, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Order>> GetByCustomerAsync(string customer, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(customer))
            return Array.Empty<Order>();

        var normalized = customer.Trim();

        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.Customer == normalized)
            .OrderByDescending(o => o.Id)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.Id)
            .ToListAsync(ct);
    }
}
