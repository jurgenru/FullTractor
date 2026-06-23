using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.DTOs.User.Response;
using FullTractor.Application.Enums;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FullTractor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<LoginResponse>>> LoginAsync([FromBody] UserRequest userRequest)
    {
        ServiceResponse<LoginResponse> loginResponse = await _authService.LoginAsync(userRequest);
        switch (loginResponse.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: loginResponse.Status.ToString());
            case Status.PasswordIncorrect:
                return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: loginResponse.Status.ToString());
            default:
                return Ok(loginResponse);
        }
    }
}