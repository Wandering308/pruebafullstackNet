using Microsoft.AspNetCore.Mvc;

namespace Prueba.Web.Controllers;

public class ReportsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ReportsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Pantalla del reporte
    [HttpGet]
    public IActionResult CustomerIntervals()
    {
        return View();
    }

    // Descarga Excel (la Web baja desde la API y devuelve el archivo)
    [HttpGet]
    public async Task<IActionResult> CustomerIntervalsExcel(CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("Api");

        // Endpoint REAL vive en la API:
        // http://localhost:5000/api/Reports/customer-intervals/excel
        var resp = await client.GetAsync("api/Reports/customer-intervals/excel", ct);

        if (!resp.IsSuccessStatusCode)
        {
            var msg = $"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}";
            return Content(msg);
        }

        var bytes = await resp.Content.ReadAsByteArrayAsync(ct);

        var contentType =
            resp.Content.Headers.ContentType?.ToString()
            ?? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fileName =
            resp.Content.Headers.ContentDisposition?.FileNameStar
            ?? resp.Content.Headers.ContentDisposition?.FileName?.Trim('"')
            ?? $"customer-intervals.xlsx";

        return File(bytes, contentType, fileName);
    }
}
