namespace FullTractor.Application.DTOs;
public class OrderResponse
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}