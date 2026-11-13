using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;
using Microsoft.AspNetCore.Mvc;
using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace FullTractor.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class CategoryController : ApiControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<CategoryResponse>>>> GetAllCategoriesAsync() =>
        ToResponse(await _categoryService.GetAllCategoriesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> GetCategoryByIdAsync([FromRoute] int id) =>
        ToResponse(await _categoryService.GetCategoryByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> CreateCategoryAsync([FromBody] CreateCategoryRequest createCategory)
    {
        ServiceResponse<CategoryResponse> categoryCreated = await _categoryService.CreateCategoryAsync(createCategory);
        if (categoryCreated.Data == null) return ToProblem(categoryCreated.Status);
        return CreatedAtAction("GetCategoryById", new { id = categoryCreated.Data.Id }, categoryCreated);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> UpdateCategoryAsync([FromRoute] int id, [FromBody] UpdateCategoryRequest updateCategory) =>
        ToResponse(await _categoryService.UpdateCategoryAsync(id, updateCategory));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> DeleteCategoryAsync([FromRoute] int id) =>
        ToResponse(await _categoryService.DeleteCategoryAsync(id));
}
