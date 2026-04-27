using Services.Dtos;

namespace Services.Interfaces;
/// <summary>
/// Сервис для работы с заказами.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Создает заказ.
    /// </summary>
    /// <param name="orderRequest">Заказ запрос.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    Task<OrderResponse> CreateOrder(OrderRequest orderRequest, CancellationToken cancelationToken);

    /// <summary>
    /// Находит заказ по номеру.
    /// </summary>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns>Заказ ответ.</returns>
    Task<OrderResponse> GetOrderByNumber(long orderNumber, CancellationToken cancelationToken);
    
    /// <summary>
    /// Формирует список всех заказов.
    /// </summary>
    /// <returns>Возвращает список со всеми заказами.</returns>
    Task<IReadOnlyList<OrderResponse>> GetAllOrders(CancellationToken cancelationToken);
    
    /// <summary>
    /// Обновляет заказ.
    /// </summary>
    /// <param name="orderRequest">Обновленная информация по заказу.</param>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task<OrderResponse> UpdateOrder(long orderNumber ,OrderRequest orderRequest, CancellationToken cancellationToken); 
    
    /// <summary>
    /// Удаляет заказ по <paramref name="orderNumber"/>.
    /// </summary>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteOrder(long orderNumber, CancellationToken cancelationToken);
}