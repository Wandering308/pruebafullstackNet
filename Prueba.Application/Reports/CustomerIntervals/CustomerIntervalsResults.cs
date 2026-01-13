namespace Prueba.Application.Reports.CustomerIntervals;

public sealed class CustomerIntervalsResults
{
    public IReadOnlyList<CustomerIntervalsReportDto> Items { get; }

    public CustomerIntervalsResults(IReadOnlyList<CustomerIntervalsReportDto> items)
        => Items = items;
}
