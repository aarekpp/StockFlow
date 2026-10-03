using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Dtos.Orders;
using StockFlow.Application.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ApiControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll(CancellationToken cancellationToken) => Ok(await _orderService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken cancellationToken) => Ok(await _orderService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto, CancellationToken cancellationToken)
    {
        var created = await _orderService.CreateAsync(dto, cancellationToken);
        return CreatedAtResult(created, nameof(GetById), o => new { id = o.Id });
    }
}
