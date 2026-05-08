namespace FullTractor.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}