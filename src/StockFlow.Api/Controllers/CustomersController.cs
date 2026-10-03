using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Dtos.Customers;
using StockFlow.Application.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ApiControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken cancellationToken) => Ok(await _customerService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken) => Ok(await _customerService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        var created = await _customerService.CreateAsync(dto, cancellationToken);
        return CreatedAtResult(created, nameof(GetById), c => new { id = c.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken)
    {
        await _customerService.UpdateAsync(id, dto, cancellationToken);
        return NoContentResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _customerService.DeleteAsync(id, cancellationToken);
        return NoContentResult();
    }
}
