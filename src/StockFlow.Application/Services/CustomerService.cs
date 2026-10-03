using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Customers;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomerService(
        IUnitOfWork unitOfWork,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<CustomerDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await GetExistingAsync(id, cancellationToken)).ToDto();
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync(cancellationToken);

        return customers.Select(c => c.ToDto()).ToList();
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_createValidator, dto, cancellationToken);

        var customer = new Customer(dto.Type, dto.Name, dto.Address, dto.Email, dto.PhoneNumber, dto.TaxId);

        await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_updateValidator, dto, cancellationToken);

        var customer = await GetExistingAsync(id, cancellationToken);

        customer.Update(dto.Name, dto.Address, dto.Email, dto.PhoneNumber, dto.TaxId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await GetExistingAsync(id, cancellationToken);

        _unitOfWork.Customers.Remove(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Customer> GetExistingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), id);
    }
}
