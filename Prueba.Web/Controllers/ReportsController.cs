using Microsoft.AspNetCore.Mvc;

namespace Prueba.Web.Controllers;

public sealed class ReportsController : Controller
{
    private readonly HttpClient _http;

    public ReportsController(HttpClient http)
    {
        _http = http;
    }

    [HttpGet]
    public IActionResult CustomerIntervals()
    {
        return View();
    }

    // Descarga desde la API y lo devuelve como archivo al browser
    [HttpGet]
    public async Task<IActionResult> CustomerIntervalsExcel(CancellationToken ct)
    {
        var resp = await _http.GetAsync("api/Reports/customer-intervals/excel", ct);
        if (!resp.IsSuccessStatusCode)
        {
            return Content($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}");
        }

        var bytes = await resp.Content.ReadAsByteArrayAsync(ct);
        var contentType = resp.Content.Headers.ContentType?.ToString()
            ?? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fileName = resp.Content.Headers.ContentDisposition?.FileName?.Trim('"')
            ?? $"customer-intervals-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

        return File(bytes, contentType, fileName);
    }
}
