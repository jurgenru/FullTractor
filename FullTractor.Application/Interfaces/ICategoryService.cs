using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;
using FullTractor.Application.DTOs.Service.Response;

namespace FullTractor.Application.Interfaces;

public interface ICategoryService
{
    public Task<ServiceResponse<List<CategoryResponse>>> GetAllCategoriesAsync();
    public Task<ServiceResponse<CategoryResponse>> GetCategoryByIdAsync(int id);
    public Task<ServiceResponse<CategoryResponse>> GetCategoryByNameAsync(string name); 
    public Task<ServiceResponse<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    public Task<ServiceResponse<CategoryResponse>> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest);
    public Task<ServiceResponse<CategoryResponse>> DeleteCategoryAsync(int id);
}