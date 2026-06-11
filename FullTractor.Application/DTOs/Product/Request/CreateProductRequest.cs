using System.ComponentModel.DataAnnotations;
using FullTractor.Application.DTOs.Category.Response;

namespace FullTractor.Application.DTOs.Product;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Name is required for product")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Category is required for product")]
    public int CategoryId { get; set; }
    [Range(0, int.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Stock must be bigger than 0")]
    public int Stock { get; set; }
    [Range(0, int.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Price must be bigger than 0")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Description is required for product")]
    public required string Description { get; set; }
}