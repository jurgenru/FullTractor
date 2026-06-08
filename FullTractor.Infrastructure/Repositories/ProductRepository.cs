using FullTractor.Domain.Entities;
using FullTractor.Domain.Exceptions;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace FullTractor.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly FullTractorContext _fullTractorContext;
    public ProductRepository(FullTractorContext fullTractorContext)
    {
        _fullTractorContext = fullTractorContext;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _fullTractorContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _fullTractorContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await _fullTractorContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _fullTractorContext.Products.AsNoTracking().Where(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        try
        {
            _fullTractorContext.Products.Add(product);
            await _fullTractorContext.SaveChangesAsync();
            return product;
        }
        catch (DbUpdateException updateEx)
        {
            if(updateEx.InnerException is SqlException upEx && (upEx.Number == 2627 || upEx.Number == 2601))
            {
                throw new DuplicateRecordException($"{product.Name} already exist");
            }
            throw;
        }
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var numUpdated = await _fullTractorContext.Products.Where(p => p.Id == product.Id).ExecuteUpdateAsync(setters => setters
                                    .SetProperty(p => p.Category, product.Category)
                                    .SetProperty(p => p.Description, product.Description)
                                    .SetProperty(p => p.Name, product.Name)
                                    .SetProperty(p => p.Price, product.Price)
                                    .SetProperty(p => p.Stock, product.Stock));
        if(numUpdated == 0) return null;
        return product;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var numDeleted = await _fullTractorContext.Products.Where(p => p.Id == productId).ExecuteDeleteAsync();
        if(numDeleted == 0) return false;
        return true;
    }
}