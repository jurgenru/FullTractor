using System.ComponentModel.DataAnnotations;

namespace FullTractor.Application.DTOs;

/// <summary>
/// The client only says what it wants to buy. The user comes from the JWT,
/// and prices, total and date are calculated on the server.
/// </summary>
public class CreateOrderRequest
{
    [MinLength(1, ErrorMessage = "The order must have at least one item")]
    public List<CreateOrderItemRequest> Items { get; set; } = [];
}
