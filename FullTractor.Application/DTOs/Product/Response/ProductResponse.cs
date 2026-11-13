using FullTractor.Application.DTOs.Category.Response;

namespace FullTractor.Application.DTOs.Product.Response;

public record ProductResponse (
    int Id, int CategoryId,
    string Name, int Stock,
    decimal Price, string Description 
);