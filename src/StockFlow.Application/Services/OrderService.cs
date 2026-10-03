using FluentValidation;
using FluentValidation.Results;
using StockFlow.Application.Common;
using StockFlow.Application.Dtos.Orders;
using StockFlow.Application.Mapping;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Enums;
using StockFlow.Domain.Exceptions;
using StockFlow.Domain.Repositories;
using System.Data;
using FluentValidationException = FluentValidation.ValidationException;

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
        var order = await _unitOfWork.Orders.GetWithItemsAsync(id, cancellationToken) ?? throw new NotFoundException(nameof(Order), id);
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
        Order? order = null;
        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId, ct) ?? throw new NotFoundException(nameof(Customer), dto.CustomerId);
            order = new Order(customer.Id, DateTime.UtcNow, dto.Notes);
            foreach (var itemDto in dto.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId, ct) ?? throw new NotFoundException(nameof(Product), itemDto.ProductId);
                if (product.StockQuantity < itemDto.Quantity) throw new DomainException($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, requested: {itemDto.Quantity}.");
                order.AddItem(product, itemDto.Quantity);
            }

            await _unitOfWork.Orders.AddAsync(order, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        var created = await _unitOfWork.Orders.GetWithItemsAsync(order!.Id, cancellationToken) ?? throw new InvalidOperationException("The order was saved but could not be reloaded immediately afterwards.");
        return created.ToDto();
    }

    public async Task<OrderDto> UpdateStatusAsync(Guid id, UpdateOrderStatusDto dto, CancellationToken cancellationToken = default)
    {
        var newStatus = ParseStatus(dto.NewStatus);
        var isolationLevel = newStatus == OrderStatus.Completed ? IsolationLevel.Serializable : IsolationLevel.ReadCommitted;

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var order = await _unitOfWork.Orders.GetWithItemsAsync(id, ct) ?? throw new NotFoundException(nameof(Order), id);
            switch (newStatus)
            {
                case OrderStatus.InProgress:
                    order.StartProcessing();
                    break;

                case OrderStatus.Completed:
                    foreach (var item in order.OrderItems)
                    {
                        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId, ct) ?? throw new NotFoundException(nameof(Product), item.ProductId);
                        product.RegisterStockMovement(StockMovementType.Issue, item.Quantity, DateTime.UtcNow, order.Id, $"Issued for completed order {order.Id}.");
                    }

                    order.Complete(DateTime.UtcNow);
                    break;

                case OrderStatus.Cancelled:
                    order.Cancel();
                    break;

                default:
                    throw new FluentValidationException(new[]
                    {
                        new ValidationFailure(nameof(UpdateOrderStatusDto.NewStatus), $"An order cannot be moved to status '{newStatus}' through this endpoint.")
                    });
            }
        }, isolationLevel, cancellationToken);

        var updated = await _unitOfWork.Orders.GetWithItemsAsync(id, cancellationToken) ?? throw new InvalidOperationException("The order was updated but could not be reloaded immediately afterwards.");
        return updated.ToDto();
    }

    private static OrderStatus ParseStatus(string value)
    {
        if (!Enum.TryParse<OrderStatus>(value, ignoreCase: true, out var status))
        {
            throw new FluentValidationException(new[]
            {
                new ValidationFailure(nameof(UpdateOrderStatusDto.NewStatus), $"'{value}' is not a recognized order status.")
            });
        }
        return status;
    }
}
