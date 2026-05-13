using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;

namespace FullTractor.Application.Services;

public class CategoryService : ICategoryService
{
    readonly ICategoryRepository _categoryRepository;
    readonly IProductRepository _productRepository;
    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categoriesResponseList = await _categoryRepository.GetAllCategoriesAsync();
        return ConvertToCategoryResponseList(categoriesResponseList);
    }
    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        var categoryResponse = await _categoryRepository.GetCategoryByIdAsync(id);
        if (categoryResponse != null) return ConvertToCategoryResponse(categoryResponse);
        return null;
    }
    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        Category categoryResponse = await _categoryRepository.CreateCategoryAsync(new Category { Name = createCategoryRequest.Name });
        return ConvertToCategoryResponse(categoryResponse);
    }
    public async Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest)
    {
        CategoryResponse? categoryResponse = await GetCategoryByIdAsync(id);
        if (categoryResponse != null)
        {
            return ConvertToCategoryResponse(await _categoryRepository.UpdateCategoryAsync(new Category { Id = id, Name = updateCategoryRequest.Name }));
        }
        return categoryResponse;
    }
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        CategoryResponse? categoryResponse = await GetCategoryByIdAsync(id);
        if (categoryResponse != null)
        {
            List<Product> productList = await _productRepository.GetProductsByCategoryIdAsync(id);
            if (productList.Count == 0) return await _categoryRepository.DeleteCategoryAsync(id);
        }
        return false;
    }
    private static List<CategoryResponse> ConvertToCategoryResponseList(List<Category> categoryList)
    {
        if (categoryList.Count == 0) return [];
        return [.. categoryList.Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })];
    }
    private static CategoryResponse ConvertToCategoryResponse(Category category)
    {
        return new CategoryResponse { Id = category.Id, Name = category.Name };
    }
}