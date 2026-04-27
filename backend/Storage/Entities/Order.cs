namespace Storage.Entities;

/// <summary>
/// Заказ.
/// </summary>
public class Order
{
    /// <summary>
    /// Id заказа.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Номер заказа.
    /// </summary>
    public long OrderNumber { get; set; }
    
    /// <summary>
    /// Город отправителя.
    /// </summary>
    public string CitySender { get; set; }
    
    /// <summary>
    /// Адрес отправителя.
    /// </summary>
    public string AddressSender { get; set;}
    
    /// <summary>
    /// Город получателя.
    /// </summary>
    public string CityReceiver { get; set;}
    
    /// <summary>
    /// Адрес получателя.
    /// </summary>
    public string AddressReceiver { get; set;}
    
    /// <summary>
    /// Вес.
    /// </summary>
    public decimal Weight { get; set;}
    
    /// <summary>
    /// Дата отправки.
    /// </summary>
    public DateOnly PickUpDate { get; set; }
    
    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreateAt { get; set; }
    
    /// <summary>
    /// Дата обновления.
    /// </summary>
    public DateTime UpdateAt { get; set; }
}