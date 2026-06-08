using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Enums;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Exceptions;
using FullTractor.Domain.Interfaces;

namespace FullTractor.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    public async Task<ServiceResponse<List<CategoryResponse>>> GetAllCategoriesAsync()
    {
        List<Category> categoriesResponseList = await _categoryRepository.GetAllCategoriesAsync();
        return ConvertToServiceListCategoryResponse(categoriesResponseList);
    }
    public async Task<ServiceResponse<CategoryResponse>> GetCategoryByIdAsync(int id)
    {
        Category? categoryResponse = await _categoryRepository.GetCategoryByIdAsync(id);
        if (categoryResponse == null) return new ServiceResponse<CategoryResponse> { Status = Status.NotFound };
        return ConvertToServiceCategoryResponse(categoryResponse);
    }
    public async Task<ServiceResponse<CategoryResponse>> GetCategoryByNameAsync(string name)
    {
        Category? category = await _categoryRepository.GetCategoryByNameAsync(name);
        if (category != null) return new ServiceResponse<CategoryResponse> { Status = Status.CategoryExists };
        return new ServiceResponse<CategoryResponse> { Status = Status.CategoryNotExists };
    }
    public async Task<ServiceResponse<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        ServiceResponse<CategoryResponse> categoryExist = await GetCategoryByNameAsync(createCategoryRequest.Name);
        if (categoryExist.Status == Status.CategoryExists) return categoryExist;
        try
        {
            Category categoryResponse = await _categoryRepository.CreateCategoryAsync(new Category { Name = createCategoryRequest.Name });
            return ConvertToServiceCategoryResponse(categoryResponse);
        }
        catch (DuplicateRecordException)
        {
            return new ServiceResponse<CategoryResponse> { Status = Status.CategoryExists };
        }
    }
    public async Task<ServiceResponse<CategoryResponse>> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest)
    {
        ServiceResponse<CategoryResponse> categoryResponse = await GetCategoryByIdAsync(id);
        if (categoryResponse.Data == null) return categoryResponse;
        var categoryUpdated = await _categoryRepository.UpdateCategoryAsync(new Category { Id = id, Name = updateCategoryRequest.Name });
        if (categoryUpdated == null) return new ServiceResponse<CategoryResponse> { Status = Status.UpdateError };
        return ConvertToServiceCategoryResponse(categoryUpdated);
    }
    public async Task<ServiceResponse<CategoryResponse>> DeleteCategoryAsync(int id)
    {
        ServiceResponse<CategoryResponse> categoryResponse = await GetCategoryByIdAsync(id);
        if (categoryResponse.Data == null) return categoryResponse;
        List<Product> productList = await _productRepository.GetProductsByCategoryIdAsync(id);
        if (productList.Count != 0) return new ServiceResponse<CategoryResponse> { Status = Status.CategoryHasProductsRelated };
        bool deleteCategory = await _categoryRepository.DeleteCategoryAsync(id);
        if (!deleteCategory) return new ServiceResponse<CategoryResponse> { Status = Status.DeleteError };
        return new ServiceResponse<CategoryResponse> { Status = Status.Success };
    }
    private static ServiceResponse<List<CategoryResponse>> ConvertToServiceListCategoryResponse(List<Category> categoryList)
    {
        if (categoryList.Count == 0) return new ServiceResponse<List<CategoryResponse>> { Status = Status.NotFound };
        return new ServiceResponse<List<CategoryResponse>> { Status = Status.Success, Data = [.. categoryList.Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })] };
    }
    private static ServiceResponse<CategoryResponse> ConvertToServiceCategoryResponse(Category category)
    {
        return new ServiceResponse<CategoryResponse> { Status = Status.Success, Data = new CategoryResponse { Id = category.Id, Name = category.Name } };
    }
}