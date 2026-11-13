using FullTractor.Application.DTOs.Product;
using FullTractor.Application.DTOs.Product.Response;
using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullTractor.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class ProductController : ApiControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<ProductResponse>>>> GetAllProductsAsync() =>
        ToResponse(await _productService.GetAllProductsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> GetProductByIdAsync([FromRoute] int id) =>
        ToResponse(await _productService.GetProductByIdAsync(id));

    [AllowAnonymous]
    [HttpGet("{categoryId}/products")]
    public async Task<ActionResult<ServiceResponse<List<ProductResponse>>>> GetProductsByCategoryIdAsync([FromRoute] int categoryId) =>
        ToResponse(await _productService.GetProductsByCategoryIdAsync(categoryId));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> CreateProductAsync([FromBody] CreateProductRequest createProduct)
    {
        ServiceResponse<ProductResponse> productCreated = await _productService.CreateProductAsync(createProduct);
        if (productCreated.Data == null) return ToProblem(productCreated.Status);
        return CreatedAtAction("GetProductById", new { id = productCreated.Data.Id }, productCreated);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> UpdateProductAsync([FromRoute] int id, [FromBody] UpdateProductRequest updateProduct) =>
        ToResponse(await _productService.UpdateProductAsync(id, updateProduct));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> DeleteProductAsync([FromRoute] int id) =>
        ToResponse(await _productService.DeleteProductAsync(id));
}
