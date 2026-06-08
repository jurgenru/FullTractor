using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
    public Task<List<Product>> GetAllProductsAsync();
    public Task<Product?> GetProductByNameAsync(string name);
    public Task<Product?> GetProductByIdAsync(int productId);
    public Task<Product> CreateProductAsync(Product product);
    public Task<Product?> UpdateProductAsync(Product product);
    public Task<bool> DeleteProductAsync(int productId);
}