using System.ComponentModel.DataAnnotations;

namespace FullTractor.Application.DTOs;

public class CreateOrderItemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be bigger than 0")]
    public int Quantity { get; set; }
}
