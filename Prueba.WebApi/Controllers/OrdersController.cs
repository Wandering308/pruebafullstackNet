using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Domain.Interfaces;
using Prueba.WebApi.Contracts;

namespace Prueba.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderService _createOrder;
    private readonly IOrderRepository _orders;

    public OrdersController(CreateOrderService createOrder, IOrderRepository orders)
    {
        _createOrder = createOrder;
        _orders = orders;
    }

    // POST: /api/orders
    [HttpPost]
    public async Task<ActionResult<CreateOrderResult>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken ct)
    {
        var cmd = new CreateOrderCommand(
            request.Customer,
            request.Product,
            request.Quantity,
            request.OriginLat,
            request.OriginLon,
            request.DestinationLat,
            request.DestinationLon
        );

        var result = await _createOrder.ExecuteAsync(cmd, ct);
        return CreatedAtAction(nameof(GetByCustomer), new { customer = result.Customer }, result);
    }

    // GET: /api/orders?customer=Juan
    [HttpGet]
    public async Task<IActionResult> GetByCustomer([FromQuery] string customer, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(customer))
            return BadRequest(new { error = "customer is required" });

        var list = await _orders.GetByCustomerAsync(customer, ct);

        // Para la consulta: cliente, producto, coords, distancia, costo (todo lo que piden ver)
        var response = list.Select(o => new
        {
            o.Id,
            o.Customer,
            o.Product,
            o.Quantity,
            OriginLat = o.Origin.Latitude,
            OriginLon = o.Origin.Longitude,
            DestinationLat = o.Destination.Latitude,
            DestinationLon = o.Destination.Longitude,
            o.DistanceKm,
            o.CostUsd
        });

        return Ok(response);
    }
}
