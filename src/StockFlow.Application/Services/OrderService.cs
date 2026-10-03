using FluentValidation;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Orders;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Exceptions;
using StockFlow.Domain.Repositories;

namespace StockFlow.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrderDto> _createValidator;

    public OrderService(IUnitOfWork unitOfWork, IValidator<CreateOrderDto> createValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
    }

    public async Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders.GetWithItemsAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), id);

        return order.ToDto();
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.Orders.GetAllWithItemsAsync(cancellationToken);

        return orders.Select(o => o.ToDto()).ToList();
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
    {
        await Guard.ValidateAsync(_createValidator, dto, cancellationToken);

        var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), dto.CustomerId);

        var order = new Order(customer.Id, DateTime.UtcNow, dto.Notes);

        foreach (var itemDto in dto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId, cancellationToken)
                ?? throw new NotFoundException(nameof(Product), itemDto.ProductId);

            if (product.StockQuantity < itemDto.Quantity)
            {
                throw new DomainException(
                    $"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, requested: {itemDto.Quantity}.");
            }

            order.AddItem(product, itemDto.Quantity);
        }

        await _unitOfWork.Orders.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var created = await _unitOfWork.Orders.GetWithItemsAsync(order.Id, cancellationToken)
            ?? throw new InvalidOperationException("The order was saved but could not be reloaded immediately afterwards.");

        return created.ToDto();
    }
}
