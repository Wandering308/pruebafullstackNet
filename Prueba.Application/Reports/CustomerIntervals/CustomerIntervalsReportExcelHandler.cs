using MediatR;

namespace Prueba.Application.Reports.CustomerIntervals;

public sealed class CustomerIntervalsReportExcelHandler
    : IRequestHandler<CustomerIntervalsReportExcelQuery, ExcelFileResult>
{
    private readonly CustomerIntervalsReportService _service;
    private readonly CustomerIntervalsExcelExporter _exporter;

    public CustomerIntervalsReportExcelHandler(
        CustomerIntervalsReportService service,
        CustomerIntervalsExcelExporter exporter)
    {
        _service = service;
        _exporter = exporter;
    }

    public async Task<ExcelFileResult> Handle(CustomerIntervalsReportExcelQuery request, CancellationToken ct)
    {
        var report = await _service.GenerateAsync(ct);
        return _exporter.Export(report);
    }
}
