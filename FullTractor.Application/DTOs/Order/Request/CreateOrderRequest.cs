namespace FullTractor.Application.DTOs;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<ProductOrderRequest> OrderItems {get; set;} = [];
}