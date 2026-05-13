using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;

namespace FullTractor.Application.Interfaces;

public interface ICategoryService
{
    public Task<List<CategoryResponse>> GetAllCategoriesAsync();
    public Task<CategoryResponse?> GetCategoryByIdAsync(int id);
    public Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    public Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest);
    public Task<bool> DeleteCategoryAsync(int id);
}