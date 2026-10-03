using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Dtos.Products;
using StockFlow.Application.Services;

namespace StockFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ApiControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken) => Ok(await _productService.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken) => Ok(await _productService.GetByIdAsync(id, cancellationToken));

    [HttpGet("below-stock/{threshold:int}")]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetBelowStockLevel(int threshold, CancellationToken cancellationToken) => Ok(await _productService.GetBelowStockLevelAsync(threshold, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto, CancellationToken cancellationToken)
    {
        var created = await _productService.CreateAsync(dto, cancellationToken);
        return CreatedAtResult(created, nameof(GetById), p => new { id = p.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        await _productService.UpdateAsync(id, dto, cancellationToken);
        return NoContentResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _productService.DeleteAsync(id, cancellationToken);
        return NoContentResult();
    }

    public async Task<ActionResult<IReadOnlyList<ProductSupplierDto>>> GetSuppliers(Guid id, CancellationToken cancellationToken) => Ok(await _productService.GetSuppliersAsync(id, cancellationToken));

    [HttpPost("{id:guid}/suppliers")]
    public async Task<ActionResult<ProductSupplierDto>> AssignSupplier(Guid id, AssignSupplierDto dto, CancellationToken cancellationToken)
    {
        var created = await _productService.AssignSupplierAsync(id, dto, cancellationToken);
        return CreatedAtAction(nameof(GetSuppliers), new { id }, created);
    }
}
