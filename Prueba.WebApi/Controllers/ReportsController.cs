using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Reports.CustomerIntervals;

namespace Prueba.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController : ControllerBase
{
    private readonly CustomerIntervalsReportService _service;

    public ReportsController(CustomerIntervalsReportService service) => _service = service;

    // GET: /api/reports/customer-intervals
    [HttpGet("customer-intervals")]
    public async Task<IActionResult> CustomerIntervals(CancellationToken ct)
    {
        var data = await _service.ExecuteAsync(ct);
        return Ok(data);
    }

    // GET: /api/reports/customer-intervals/excel
    [HttpGet("customer-intervals/excel")]
    public async Task<IActionResult> CustomerIntervalsExcel(CancellationToken ct)
    {
        var data = await _service.ExecuteAsync(ct);
        var bytes = CustomerIntervalsExcelExporter.Export(data);

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "customer-intervals-report.xlsx"
        );
    }
}
