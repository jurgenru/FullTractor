using FullTractor.Application.DTOs.Service;
using FullTractor.Application.DTOs.Product.Response;
using FullTractor.Application.DTOs.Product;

namespace FullTractor.Application.Interfaces;

public interface IProductService
{
    public Task<ServiceResponse<List<ProductResponse>>> GetAllProductsAsync();
    public Task<ServiceResponse<ProductResponse>> GetProductByIdAsync(int productId);
    public Task<ServiceResponse<List<ProductResponse>>> GetProductsByCategoryIdAsync(int categoryId);
    public Task<ServiceResponse<ProductResponse>> UpdateProductAsync(int productId, UpdateProductRequest updateProduct);
    public Task<ServiceResponse<ProductResponse>> CreateProductAsync(CreateProductRequest createProduct);
    public Task<ServiceResponse<ProductResponse>> DeleteProductAsync(int productId);

}