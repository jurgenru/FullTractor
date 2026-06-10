namespace FullTractor.Application.DTOs;

public class OrderItemResponse
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public required string Name { get; set; }
    public decimal HistoricalPrice { get; set; }
    public int Quantity { get; set; }
}