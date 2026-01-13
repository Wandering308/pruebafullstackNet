using Prueba.Domain.Interfaces;

namespace Prueba.Application.Reports.CustomerIntervals;

public sealed class CustomerIntervalsReportService
{
    private readonly IOrderRepository _repo;

    public CustomerIntervalsReportService(IOrderRepository repo)
        => _repo = repo;

    public async Task<CustomerIntervalsResults> GenerateAsync(CancellationToken ct = default)
    {
        var orders = await _repo.GetAllAsync(ct);

        var rows = orders
            .GroupBy(o => o.Customer)
            .Select(g =>
            {
                var from1to50 = g.Count(o => o.DistanceKm >= 1 && o.DistanceKm <= 50);
                var from51to200 = g.Count(o => o.DistanceKm >= 51 && o.DistanceKm <= 200);
                var from201to500 = g.Count(o => o.DistanceKm >= 201 && o.DistanceKm <= 500);
                var from501to1000 = g.Count(o => o.DistanceKm >= 501 && o.DistanceKm <= 1000);

                var total = from1to50 + from51to200 + from201to500 + from501to1000;

                return new CustomerIntervalsReportDto(
                    g.Key,
                    from1to50,
                    from51to200,
                    from201to500,
                    from501to1000,
                    total
                );
            })
            .OrderBy(x => x.Customer)
            .ToList();

        return new CustomerIntervalsResults(rows);
    }
}
