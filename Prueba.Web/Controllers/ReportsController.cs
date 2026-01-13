using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Prueba.Web.Controllers;

public sealed class ReportsController : Controller
{
    private readonly HttpClient _http;

    public ReportsController(HttpClient http)
    {
        _http = http;
    }

    // GET: /Reports/CustomerIntervals
    [HttpGet]
    public IActionResult CustomerIntervals()
    {
        return View();
    }

    // GET: /Reports/CustomerIntervals/Excel
    // Descarga desde la API y devuelve como archivo al browser
    [HttpGet]
    public async Task<IActionResult> CustomerIntervalsExcel(CancellationToken ct)
    {
        // ✅ IMPORTANTE: ruta correcta (en minúscula y consistente)
        var apiPath = "api/reports/customer-intervals/excel";

        using var resp = await _http.GetAsync(apiPath, HttpCompletionOption.ResponseHeadersRead, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            return Content($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}\n\n{body}");
        }

        var bytes = await resp.Content.ReadAsByteArrayAsync(ct);

        var contentType =
            resp.Content.Headers.ContentType?.ToString()
            ?? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // Intenta sacar el filename del header (si existe)
        string fileName =
            resp.Content.Headers.ContentDisposition?.FileNameStar
            ?? resp.Content.Headers.ContentDisposition?.FileName
            ?? $"customer-intervals-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

        fileName = fileName.Trim('"');

        return File(bytes, contentType, fileName);
    }
}
