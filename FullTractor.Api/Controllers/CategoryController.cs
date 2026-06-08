using FullTractor.Application.DTOs.Category.Request;
using FullTractor.Application.DTOs.Category.Response;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Interfaces;
using FullTractor.Application.Enums;
using Microsoft.AspNetCore.Mvc;
namespace FullTractor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<CategoryResponse>>>> GetAllCategoriesAsync()
    {
        ServiceResponse<List<CategoryResponse>> categoryList = await _categoryService.GetAllCategoriesAsync();
        if(categoryList.Status != Status.Success) return Problem(statusCode: StatusCodes.Status404NotFound, detail: "");
        return Ok(categoryList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> GetCategoryByIdAsync([FromRoute] int id)
    {
        ServiceResponse<CategoryResponse> getCategory = await _categoryService.GetCategoryByIdAsync(id);
        if (getCategory.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: getCategory.Status.ToString(), title: "Category was not possible to find");
        return Ok(getCategory);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> CreateCategoryAsync([FromBody] CreateCategoryRequest createCategory)
    {
        ServiceResponse<CategoryResponse> categoryCreated = await _categoryService.CreateCategoryAsync(createCategory);
        switch (categoryCreated.Status)
        {
            case Status.CategoryExists:
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: categoryCreated.Status.ToString());
            default:
                return CreatedAtAction(nameof(GetCategoryByIdAsync), new {id = categoryCreated.Data?.Id}, categoryCreated);
        }        
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> UpdateCategoryAsync([FromRoute]int id, [FromBody]UpdateCategoryRequest updateCategory)
    {
        ServiceResponse<CategoryResponse> categoryUpdated = await _categoryService.UpdateCategoryAsync(id, updateCategory);
        switch (categoryUpdated.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: categoryUpdated.Status.ToString());
            case Status.UpdateError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: categoryUpdated.Status.ToString());
            default:
                return Ok(categoryUpdated);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<CategoryResponse>>> DeleteCategoryAsync([FromRoute]int id)
    {
        ServiceResponse<CategoryResponse> categoryDeleted = await _categoryService.DeleteCategoryAsync(id);
        switch (categoryDeleted.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: categoryDeleted.Status.ToString());
            case Status.DeleteError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: categoryDeleted.Status.ToString());
            default:
                return Ok(categoryDeleted);
        }
    }
}