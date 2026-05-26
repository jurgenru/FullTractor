using FullTractor.Application.DTOs.Product;
using FullTractor.Application.DTOs.Product.Response;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Application.Enums;
using FullTractor.Application.DTOs.Category.Response;

namespace FullTractor.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ServiceResponse<List<ProductResponse>>> GetAllProductsAsync()
    {
        List<Product> listProduct = await _productRepository.GetAllProductsAsync();
        return ConvertToProductList(listProduct);
    }

    public async Task<ServiceResponse<ProductResponse>> GetProductByIdAsync(int productId)
    {
        Product? product = await _productRepository.GetProductByIdAsync(productId);
        if (product == null) return new ServiceResponse<ProductResponse> { Status = Status.NotFound };
        return ConvertToProductResponse(product);
    }
    public async Task<ServiceResponse<List<ProductResponse>>> GetProductsByCategoryIdAsync(int categoryId)
    {
        List<Product> productList = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        if(productList.Count == 0) return new ServiceResponse<List<ProductResponse>> {Status = Status.NotFound};
        return ConvertToProductList(productList);
    }
    public async Task<ServiceResponse<ProductResponse>> CreateProductAsync(CreateProductRequest createProduct)
    {
        Product product = await _productRepository.CreateProductAsync(new Product { Name = createProduct.Name, Description = createProduct.Description, Price = createProduct.Price, Stock = createProduct.Stock, CategoryId = createProduct.Category.Id, Category = new Category { Name = createProduct.Category.Name, Id = createProduct.Category.Id } });
        return ConvertToProductResponse(product);
    }

    public async Task<ServiceResponse<ProductResponse>> UpdateProductAsync(int productId, UpdateProductRequest updateProduct)
    {
        ServiceResponse<ProductResponse> productExist = await GetProductByIdAsync(productId);
        if (productExist.Data == null) return productExist;
        Product? productUpdate = await _productRepository.UpdateProductAsync(new Product { Name = updateProduct.Name, Description = updateProduct.Description, Price = updateProduct.Price, Stock = updateProduct.Stock, Category = new Category { Name = updateProduct.Category.Name, Id = updateProduct.Category.Id }, CategoryId = updateProduct.Category.Id });
        if (productUpdate == null) return new ServiceResponse<ProductResponse> { Status = Status.UpdateError };
        return ConvertToProductResponse(productUpdate);
    }

    public async Task<ServiceResponse<ProductResponse>> DeleteProductAsync(int productId)
    {
        ServiceResponse<ProductResponse> productExist = await GetProductByIdAsync(productId);
        if (productExist.Data == null) return productExist;
        bool productDelete = await _productRepository.DeleteProductAsync(productId);
        if (!productDelete) return new ServiceResponse<ProductResponse> { Status = Status.DeleteError };
        return new ServiceResponse<ProductResponse> { Status = Status.Success };
    }

    private ServiceResponse<List<ProductResponse>> ConvertToProductList(List<Product> productList)
    {
        if (productList.Count == 0) return new ServiceResponse<List<ProductResponse>> { Status = Status.NotFound };
        return new ServiceResponse<List<ProductResponse>> { Status = Status.Success, Data = [.. productList.Select(p => new ProductResponse { Name = p.Name, Description = p.Description, Stock = p.Stock, Price = p.Price, Category = new CategoryResponse { Name = p.Category.Name, Id = p.Category.Id } })] };
    }
    private ServiceResponse<ProductResponse> ConvertToProductResponse(Product product)
    {
        return new ServiceResponse<ProductResponse> { Status = Status.Success, Data = new ProductResponse { Description = product.Description, Name = product.Name, Price = product.Price, Stock = product.Stock, Category = new CategoryResponse { Name = product.Category.Name, Id = product.Category.Id } } };
    }
}