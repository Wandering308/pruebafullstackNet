using Microsoft.AspNetCore.Mvc;
using Prueba.Web.Models;
using System.Net.Http.Json;

namespace Prueba.Web.Controllers;

public sealed class OrdersController : Controller
{
    private readonly HttpClient _http;

    public OrdersController(HttpClient http)
    {
        _http = http;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? customer)
    {
        var vm = new OrdersByCustomerVm
        {
            Customer = customer ?? ""
        };

        if (string.IsNullOrWhiteSpace(customer))
            return View(vm);

        try
        {
            var url = $"api/Orders?customer={Uri.EscapeDataString(customer)}";
            var data = await _http.GetFromJsonAsync<List<OrderDto>>(url);

            vm.Orders = data ?? new List<OrderDto>();
        }
        catch (HttpRequestException ex)
        {
            vm.Error = $"API error: {ex.Message}";
        }
        catch (Exception ex)
        {
            vm.Error = $"Unexpected error: {ex.Message}";
        }

        return View(vm);
    }
}
