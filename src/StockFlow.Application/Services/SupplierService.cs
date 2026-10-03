using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Suppliers;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSupplierDto> _createValidator;
    private readonly IValidator<UpdateSupplierDto> _updateValidator;

    public SupplierService(
        IUnitOfWork unitOfWork,
        IValidator<CreateSupplierDto> createValidator,
        IValidator<UpdateSupplierDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<SupplierDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await GetExistingAsync(id, cancellationToken)).ToDto();
    }

    public async Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync(cancellationToken);

        return suppliers.Select(s => s.ToDto()).ToList();
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_createValidator, dto, cancellationToken);

        var supplier = new Supplier(dto.Name, dto.Address, dto.PhoneNumber, dto.Email, dto.TaxId);

        await _unitOfWork.Suppliers.AddAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return supplier.ToDto();
    }

    public async Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_updateValidator, dto, cancellationToken);

        var supplier = await GetExistingAsync(id, cancellationToken);
        supplier.Update(dto.Name, dto.Address, dto.PhoneNumber, dto.Email, dto.TaxId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return supplier.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var supplier = await GetExistingAsync(id, cancellationToken);

        _unitOfWork.Suppliers.Remove(supplier);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Supplier> GetExistingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Suppliers.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Supplier), id);
    }
}
