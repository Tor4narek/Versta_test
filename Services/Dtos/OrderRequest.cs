namespace Services.Dtos;

/// <summary>
/// Заказ запрос.
/// </summary>
public class OrderRequest
{
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

}