namespace Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Storage;
using Storage.Entities;
using Interfaces;
using Dtos;

/// <inheritdoc/>
public class OrderService : IOrderService
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Создает объект.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <exception cref="ArgumentException">Если <paramref name="dbContext"/> null.</exception>
    public OrderService(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }
    
    /// <inheritdoc/>
    public async Task<OrderResponse> CreateOrder(OrderRequest orderRequest, CancellationToken cancelationToken)
    {
        ValidateOrder(orderRequest);

        var order = new Order()
        {
            Id = Guid.NewGuid(),
            AddressSender = orderRequest.AddressSender,
            CitySender = orderRequest.CitySender,
            AddressReceiver = orderRequest.AddressReceiver,
            CityReceiver = orderRequest.CityReceiver,
            PickUpDate = orderRequest.PickUpDate,
            Weight = orderRequest.Weight,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        
        await _dbContext.Orders.AddAsync(order, cancelationToken);
        await _dbContext.SaveChangesAsync(cancelationToken);
        
        return new OrderResponse
        {
            OrderNumber = order.OrderNumber,
            AddressSender = order.AddressSender,
            CitySender = order.CitySender,
            AddressReceiver = order.AddressReceiver,
            CityReceiver = order.CityReceiver,
            Weight = order.Weight,
            PickUpDate = order.PickUpDate
        };
    }
    
    /// <inheritdoc/>
    public async Task<OrderResponse> GetOrderByNumber(long orderNumber, CancellationToken cancelationToken)
    {
        if (orderNumber <= 0)
        {
            throw new IndexOutOfRangeException($"Номер заказа не может быть меньше либо равен 0");
        }
        
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Where(x => x.OrderNumber == orderNumber)
            .Select(x => new OrderResponse()
            {
                OrderNumber = x.OrderNumber,
                AddressSender =  x.AddressSender,
                CitySender = x.CitySender,
                AddressReceiver = x.AddressReceiver,
                CityReceiver = x.CityReceiver,
                Weight = x.Weight,
                PickUpDate = x.PickUpDate,
            })
            .FirstOrDefaultAsync(cancelationToken);

        return order ?? throw new ArgumentException($"Заказа с номером {orderNumber} не существует.");
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<OrderResponse>> GetAllOrders(CancellationToken cancelationToken)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Select(x => new OrderResponse()
            {
                OrderNumber = x.OrderNumber,
                AddressSender =  x.AddressSender,
                CitySender = x.CitySender,
                AddressReceiver = x.AddressReceiver,
                CityReceiver = x.CityReceiver,
                Weight = x.Weight,
                PickUpDate = x.PickUpDate,
            })
            .ToListAsync(cancelationToken);
    }

    /// <inheritdoc/>
    public async Task<OrderResponse> UpdateOrder(long orderNumber, OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        if (orderNumber <= 0)
        {
            throw new IndexOutOfRangeException($"Номер заказа не может быть меньше либо равен 0");
        }
        
        ValidateOrder(orderRequest);
        
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Where(x => x.OrderNumber == orderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException($"Заказа с номером {orderNumber} не существует.");
        }
        
        _dbContext.Orders.Update(new Order()
        {
            Id = order.Id,
            OrderNumber = orderNumber,
            AddressSender = orderRequest.AddressSender,
            CitySender = orderRequest.CitySender,
            AddressReceiver = orderRequest.AddressReceiver,
            CityReceiver = orderRequest.CityReceiver,
            Weight = orderRequest.Weight,
            PickUpDate = orderRequest.PickUpDate,
            CreateAt = order.CreateAt,
            UpdateAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new OrderResponse()
        {
            OrderNumber = orderNumber,
            AddressSender = orderRequest.AddressSender,
            CitySender = orderRequest.CitySender,
            AddressReceiver = orderRequest.AddressReceiver,
            CityReceiver = orderRequest.CityReceiver,
            Weight = orderRequest.Weight,
            PickUpDate = orderRequest.PickUpDate
        };

    }

    /// <inheritdoc/>
    public async Task DeleteOrder(long orderNumber, CancellationToken cancelationToken)
    { 
        if (orderNumber <= 0)
        {
            throw new IndexOutOfRangeException($"Номер заказа не может быть меньше либо равен 0");
        }
        
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Where(x => x.OrderNumber == orderNumber)
            .FirstOrDefaultAsync(cancelationToken);

        if (order == null)
        {
            throw new KeyNotFoundException($"Заказа с номером {orderNumber} не существует.");
        }
        
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancelationToken);
    }

    /// <summary>
    /// Валидирует <paramref name="orderRequest"/>.
    /// </summary>
    /// <param name="orderRequest">Заказ запрос.</param>
    /// <exception cref="ArgumentOutOfRangeException">Если вес меньше 0.</exception>
    /// <exception cref="ValidationException">Если дата в прошлом.</exception>
    /// <exception cref="ArgumentException">Если строки пустые или null.</exception>
    /// <exception cref="ArgumentNullException">Если <paramref name="orderRequest"/> null.</exception>
    private void ValidateOrder(OrderRequest orderRequest)
    {
        ArgumentNullException.ThrowIfNull(orderRequest);
        ArgumentException.ThrowIfNullOrWhiteSpace(orderRequest.AddressReceiver);
        ArgumentException.ThrowIfNullOrWhiteSpace(orderRequest.CityReceiver);
        ArgumentException.ThrowIfNullOrWhiteSpace(orderRequest.AddressSender);
        ArgumentException.ThrowIfNullOrWhiteSpace(orderRequest.CitySender);
        
        if (orderRequest.Weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(orderRequest.Weight),"Вес не может быть меньше либо равен 0.");
        }
    }
    
}