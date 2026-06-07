using FullTractor.Domain.Entities;
namespace FullTractor.Domain.Interfaces;

public interface ICategoryRepository
{
    public Task<List<Category>> GetAllCategoriesAsync();
    public Task<Category?> GetCategoryByIdAsync(int categoryId);
    public Task<Category?> GetCategoryByNameAsync(string name);
    public Task<Category> CreateCategoryAsync(Category category);
    public Task<Category?> UpdateCategoryAsync(Category category);
    public Task<bool> DeleteCategoryAsync(int categoryId);
}