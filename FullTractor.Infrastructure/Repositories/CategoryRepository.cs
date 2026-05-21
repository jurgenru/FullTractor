using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace FullTractor.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FullTractorContext _fullTractorContext;
    public CategoryRepository(FullTractorContext fullTractorContext)
    {
        _fullTractorContext = fullTractorContext;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _fullTractorContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        return await _fullTractorContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        try
        {
            _fullTractorContext.Add(category);
            await _fullTractorContext.SaveChangesAsync();
            return category;
        }
        catch (DbUpdateException updateException)
        {
            throw new Exception("Unable to create category. Verify if the properties are correct or the database connection.", updateException);
        }
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        try
        {
            await _fullTractorContext.Categories.Where(c => c.Id == category.Id).ExecuteUpdateAsync(setters => setters.SetProperty(c => c.Name, category.Name));
            return category;
        }
        catch (DbUpdateException updateException)
        {
            throw new Exception("Unable to update category. Verify if the properties are correct or the database connection.", updateException);
        }
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        try
        {
            await _fullTractorContext.Categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync();
            return true;
        }
        catch (DbUpdateException updateException)
        {
            throw new Exception("Unable to delete category. Verify the database connection", updateException);
        }
    }
}