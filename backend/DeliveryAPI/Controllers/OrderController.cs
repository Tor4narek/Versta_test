using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;

namespace DeliveryAPI.Controllers;

/// <summary>
/// Контроллер для работы с заказами.
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    /// <summary>
    /// Сервис заказов.
    /// </summary>
    private readonly IOrderService _orderService;

    /// <summary>
    /// Создает контроллер заказов.
    /// </summary>
    /// <param name="orderService">Сервис заказов.</param>
    public OrderController(IOrderService orderService)
    {
        ArgumentNullException.ThrowIfNull(orderService);
        _orderService = orderService;
    }

    /// <summary>
    /// Создает новый заказ.
    /// </summary>
    /// <param name="orderRequest">Данные заказа.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns>Созданный заказ.</returns>
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        [FromBody] OrderRequest orderRequest,
        CancellationToken cancelationToken)
    {
        try
        {
            var createdOrder = await _orderService.CreateOrder(orderRequest, cancelationToken);

            return CreatedAtAction(
                nameof(GetOrderByNumber),
                new { orderNumber = createdOrder.OrderNumber },
                createdOrder);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Возвращает список всех заказов.
    /// </summary>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns>Список заказов.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetAllOrders(
        CancellationToken cancelationToken)
    {
        var orders = await _orderService.GetAllOrders(cancelationToken);

        return Ok(orders);
    }

    /// <summary>
    /// Возвращает заказ по номеру.
    /// </summary>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns>Заказ.</returns>
    [HttpGet("{orderNumber:long}")]
    public async Task<ActionResult<OrderResponse>> GetOrderByNumber(
        long orderNumber,
        CancellationToken cancelationToken)
    {
        try
        {
            var order = await _orderService.GetOrderByNumber(orderNumber, cancelationToken);

            return Ok(order);
        }
        catch (IndexOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Обновляет заказ по номеру.
    /// </summary>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="orderRequest">Новые данные заказа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленный заказ.</returns>
    [HttpPut("{orderNumber:long}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(
        long orderNumber,
        [FromBody] OrderRequest orderRequest,
        CancellationToken cancellationToken)
    {
        try
        {
            var updatedOrder = await _orderService.UpdateOrder(
                orderNumber,
                orderRequest,
                cancellationToken);

            return Ok(updatedOrder);
        }
        catch (IndexOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет заказ по номеру.
    /// </summary>
    /// <param name="orderNumber">Номер заказа.</param>
    /// <param name="cancelationToken">Токен отмены.</param>
    /// <returns>Результат удаления.</returns>
    [HttpDelete("{orderNumber:long}")]
    public async Task<IActionResult> DeleteOrder(
        long orderNumber,
        CancellationToken cancelationToken)
    {
        try
        {
            await _orderService.DeleteOrder(orderNumber, cancelationToken);

            return NoContent();
        }
        catch (IndexOutOfRangeException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}