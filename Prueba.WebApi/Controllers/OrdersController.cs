using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Orders.CreateOrder;
using Prueba.Application.Orders.GetOrdersByCustomer;
using Prueba.WebApi.Contracts;

namespace Prueba.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: /api/orders
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        // 201 Created + body (sin forzar un Get que no corresponde a un recurso único)
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // GET: /api/orders?customer=Felipe
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer([FromQuery] string customer, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrdersByCustomerQuery(customer), ct);
        return Ok(result);
    }
}
