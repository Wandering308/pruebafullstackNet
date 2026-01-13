using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Prueba.Web.Models;

namespace Prueba.Web.Controllers;

public sealed class OrdersController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

    public OrdersController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    [HttpGet]
    public IActionResult Index() => View(new OrdersByCustomerVm());

    [HttpPost]
    public async Task<IActionResult> Index(OrdersByCustomerVm vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Customer))
        {
            vm.Error = "Customer is required.";
            return View(vm);
        }

        var client = _httpClientFactory.CreateClient("Api");

        try
        {
            var resp = await client.GetAsync($"/api/orders?customer={Uri.EscapeDataString(vm.Customer)}");

            if (!resp.IsSuccessStatusCode)
            {
                vm.Error = $"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}";
                return View(vm);
            }

            var json = await resp.Content.ReadAsStringAsync();
            vm.Orders = JsonSerializer.Deserialize<List<OrderVm>>(json, _jsonOpts) ?? new();
            return View(vm);
        }
        catch (Exception ex)
        {
            vm.Error = ex.Message;
            return View(vm);
        }
    }
}
