namespace FullTractor.Application.DTOs;
public class ProductOrderRequest
{
    public int ProductId { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
    public decimal HistoricalPrice { get; set; }
}