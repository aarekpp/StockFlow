using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Dtos.Suppliers;
using StockFlow.Application.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ApiControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetAll(CancellationToken cancellationToken) => Ok(await _supplierService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id, CancellationToken cancellationToken) => Ok(await _supplierService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create(CreateSupplierDto dto, CancellationToken cancellationToken)
    {
        var created = await _supplierService.CreateAsync(dto, cancellationToken);
        return CreatedAtResult(created, nameof(GetById), s => new { id = s.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateSupplierDto dto, CancellationToken cancellationToken)
    {
        await _supplierService.UpdateAsync(id, dto, cancellationToken);
        return NoContentResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _supplierService.DeleteAsync(id, cancellationToken);
        return NoContentResult();
    }
}
