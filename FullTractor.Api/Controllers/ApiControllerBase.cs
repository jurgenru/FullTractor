using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FullTractor.Api.Controllers;

/// <summary>
/// Maps every service Status to an HTTP response in one place,
/// so an unhandled status can never come back as a 200.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<ServiceResponse<T>> ToResponse<T>(ServiceResponse<T> response) where T : class
    {
        if (response.Status == Status.Success) return Ok(response);
        return ToProblem(response.Status);
    }

    protected ObjectResult ToProblem(Status status)
    {
        int statusCode = status switch
        {
            Status.NotFound or Status.OrderItemNotExist or Status.ProductNotExists or Status.CategoryNotExists
                => StatusCodes.Status404NotFound,
            Status.PasswordIncorrect
                => StatusCodes.Status401Unauthorized,
            Status.EmailExists or Status.ProductExists or Status.CategoryExists
                or Status.CategoryHasProductsRelated or Status.InsufficientStock
                => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, detail: status.ToString());
    }
}
