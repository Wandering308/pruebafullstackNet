using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Reports.CustomerIntervals;

namespace Prueba.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: /api/reports/customer-intervals/excel
    [HttpGet("customer-intervals/excel")]
    public async Task<IActionResult> CustomerIntervalsExcel(CancellationToken ct)
    {
        var file = await _mediator.Send(new CustomerIntervalsReportExcelQuery(), ct);
        return File(file.Content, file.ContentType, file.FileName);
    }
}
