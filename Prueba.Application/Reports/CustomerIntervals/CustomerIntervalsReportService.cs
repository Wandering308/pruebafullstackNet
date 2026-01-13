using Prueba.Domain.Interfaces;

namespace Prueba.Application.Reports.CustomerIntervals;

public sealed class CustomerIntervalsReportService
{
    private readonly IOrderRepository _orders;

    public CustomerIntervalsReportService(IOrderRepository orders) => _orders = orders;

    public async Task<IReadOnlyList<CustomerIntervalsReportDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var all = await _orders.GetAllAsync(ct);

        var report = all
            .GroupBy(o => o.Customer)
            .Select(g =>
            {
                int c1 = 0, c2 = 0, c3 = 0, c4 = 0;

                foreach (var o in g)
                {
                    var d = o.DistanceKm;

                    if (d <= 50) c1++;
                    else if (d <= 200) c2++;
                    else if (d <= 500) c3++;
                    else c4++; // 501..1000
                }

                return new CustomerIntervalsReportDto(g.Key, c1, c2, c3, c4);
            })
            .OrderBy(x => x.Customer)
            .ToList();

        return report;
    }
}
