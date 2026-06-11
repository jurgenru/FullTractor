using FullTractor.Application.DTOs.Product;
using FullTractor.Application.DTOs.Product.Response;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Enums;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<ProductResponse>>>> GetAllProductsAsync()
    {
        ServiceResponse<List<ProductResponse>> productList = await _productService.GetAllProductsAsync();
        if (productList.Status != Status.Success) return Problem(statusCode: StatusCodes.Status404NotFound, detail: productList.Status.ToString());
        return Ok(productList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> GetProductByIdAsync([FromRoute] int id)
    {
        ServiceResponse<ProductResponse> product = await _productService.GetProductByIdAsync(id);
        if (product.Status != Status.Success) return Problem(statusCode: StatusCodes.Status404NotFound, detail: product.Status.ToString());
        return Ok(product);
    }

    [HttpGet("{categoryId}/products")]
    public async Task<ActionResult<ServiceResponse<List<ProductResponse>>>> GetProductsByCategoryIdAsync([FromRoute] int categoryId)
    {
        ServiceResponse<List<ProductResponse>> productList = await _productService.GetProductsByCategoryIdAsync(categoryId);
        if (productList.Status != Status.Success) return Problem(statusCode: StatusCodes.Status404NotFound, detail: productList.Status.ToString());
        return Ok(productList);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> CreateProductAsync([FromBody] CreateProductRequest createProduct)
    {
        ServiceResponse<ProductResponse> productCreated = await _productService.CreateProductAsync(createProduct);
        switch (productCreated.Status)
        {
            case Status.ProductExists:
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: productCreated.Status.ToString());
            default:
                return CreatedAtAction("GetProductById", new { id = productCreated.Data?.Id }, productCreated);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> UpdateProductAsynct([FromRoute] int id, [FromBody] UpdateProductRequest updateProduct)
    {
        ServiceResponse<ProductResponse> productUpdated = await _productService.UpdateProductAsync(id, updateProduct);
        switch (productUpdated.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: productUpdated.Status.ToString());
            case Status.UpdateError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: productUpdated.Status.ToString());
            default:
                return Ok(productUpdated);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<ProductResponse>>> DeleteProductAsync([FromRoute] int id)
    {
        ServiceResponse<ProductResponse> productDelete = await _productService.DeleteProductAsync(id);
        switch (productDelete.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: productDelete.Status.ToString());
            case Status.DeleteError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: productDelete.Status.ToString());
            default:
                return Ok(productDelete);
        }
    }
}