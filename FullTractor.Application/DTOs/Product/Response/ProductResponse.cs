namespace FullTractor.Application.DTOs.Product.Response;

public class ProductResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // public required Category Category { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
}