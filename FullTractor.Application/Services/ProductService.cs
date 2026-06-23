using FullTractor.Application.DTOs.Product;
using FullTractor.Application.DTOs.Product.Response;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Application.Enums;
using FullTractor.Domain.Exceptions;
using FullTractor.Application.DTOs.Service.Response;

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
    public async Task<ServiceResponse<ProductResponse>> GetProductByNameAsync(string name)
    {
        Product? product = await _productRepository.GetProductByNameAsync(name);
        if (product != null) return new ServiceResponse<ProductResponse> { Status = Status.ProductExists };
        return new ServiceResponse<ProductResponse> { Status = Status.ProductNotExists };
    }
    public async Task<ServiceResponse<List<ProductResponse>>> GetProductsByCategoryIdAsync(int categoryId)
    {
        List<Product> productList = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        if (productList.Count == 0) return new ServiceResponse<List<ProductResponse>> { Status = Status.NotFound };
        return ConvertToProductList(productList);
    }
    public async Task<ServiceResponse<ProductResponse>> CreateProductAsync(CreateProductRequest createProduct)
    {
        ServiceResponse<ProductResponse> productExist = await GetProductByNameAsync(createProduct.Name);
        if (productExist.Status == Status.ProductExists) return productExist;
        try
        {
            Product product = await _productRepository.CreateProductAsync(new Product { Name = createProduct.Name, Description = createProduct.Description, Price = createProduct.Price, Stock = createProduct.Stock, CategoryId = createProduct.CategoryId });
            return ConvertToProductResponse(product);
        }
        catch (DuplicateRecordException)
        {
            return new ServiceResponse<ProductResponse> { Status = Status.ProductExists };
        }
    }

    public async Task<ServiceResponse<ProductResponse>> UpdateProductAsync(int productId, UpdateProductRequest updateProduct)
    {
        ServiceResponse<ProductResponse> productExist = await GetProductByIdAsync(productId);
        if (productExist.Data == null) return productExist;
        Product? productUpdate = await _productRepository.UpdateProductAsync(new Product { Id = productId, Name = updateProduct.Name, Description = updateProduct.Description, Price = updateProduct.Price, Stock = updateProduct.Stock, CategoryId = updateProduct.CategoryId });
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
        return new ServiceResponse<List<ProductResponse>> { Status = Status.Success, Data = [.. productList.Select(p => new ProductResponse { Id = p.Id, Name = p.Name, Description = p.Description, Stock = p.Stock, Price = p.Price, CategoryId = p.CategoryId })] };
    }
    private ServiceResponse<ProductResponse> ConvertToProductResponse(Product product)
    {
        return new ServiceResponse<ProductResponse> { Status = Status.Success, Data = new ProductResponse { Id = product.Id, Description = product.Description, Name = product.Name, Price = product.Price, Stock = product.Stock, CategoryId = product.CategoryId } };
    }
}