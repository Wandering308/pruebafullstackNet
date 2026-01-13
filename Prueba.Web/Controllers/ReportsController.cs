using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Prueba.Web.Models;

namespace Prueba.Web.Controllers;

public sealed class ReportsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public ReportsController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    // GET: /Reports/CustomerIntervals
    [HttpGet]
    public async Task<IActionResult> CustomerIntervals()
    {
        var client = _httpClientFactory.CreateClient("Api");

        try
        {
            var resp = await client.GetAsync("/api/reports/customer-intervals");
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = $"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}";
                return View(new List<CustomerIntervalsReportVm>());
            }

            var json = await resp.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CustomerIntervalsReportVm>>(json, _jsonOpts) ?? new();
            return View(data);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(new List<CustomerIntervalsReportVm>());
        }
    }

    // GET: /Reports/CustomerIntervalsExcel
    [HttpGet]
    public async Task<IActionResult> CustomerIntervalsExcel()
    {
        var client = _httpClientFactory.CreateClient("Api");

        var resp = await client.GetAsync("/api/reports/customer-intervals/excel");
        if (!resp.IsSuccessStatusCode)
            return BadRequest($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}");

        var bytes = await resp.Content.ReadAsByteArrayAsync();

        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "customer-intervals-report.xlsx"
        );
    }
}
