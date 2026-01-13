using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Domain.Interfaces;
using Prueba.WebApi.Contracts;

namespace Prueba.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderRepository _orders;

    public OrdersController(IMediator mediator, IOrderRepository orders)
    {
        _mediator = mediator;
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

        var result = await _mediator.Send(cmd, ct);

        // mantiene el mismo Location que ya venías usando
        return CreatedAtAction(nameof(GetByCustomer), new { customer = result.Customer }, result);
    }

    // GET: /api/orders?customer=Felipe
    [HttpGet]
    public async Task<IActionResult> GetByCustomer(
        [FromQuery] string customer,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(customer))
            return BadRequest(new { error = "customer is required" });

        var list = await _orders.GetByCustomerAsync(customer, ct);
        return Ok(list);
    }
}
