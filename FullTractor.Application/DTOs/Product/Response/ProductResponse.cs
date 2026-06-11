using FullTractor.Application.DTOs.Category.Response;

namespace FullTractor.Application.DTOs.Product.Response;

public class ProductResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
}