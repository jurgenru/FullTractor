using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FullTractor.Application.Enums;
using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Response;
using FullTractor.Application.DTOs.User.Request;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> GetUserByIdAsync([FromRoute] int id)
    {
        ServiceResponse<UserResponse> user = await _userService.GetUserByIdAsync(id);
        if (user.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: user.Status.ToString());
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> GetUserByEmailAsync([FromBody] string email)
    {
        ServiceResponse<UserResponse> user = await _userService.GetUserByEmailAsync(email);
        if (user.Status == Status.NotFound) return Problem(statusCode: StatusCodes.Status404NotFound, detail: user.Status.ToString());
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> CreateUserAsync([FromBody] UserRequest userRequest)
    {
        ServiceResponse<UserResponse> userCreate = await _userService.CreateUserAsync(userRequest);
        switch (userCreate.Status)
        {
            case Status.EmailExists:
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: userCreate.Status.ToString());
            default:
                return CreatedAtAction("GetUserById", new { id = userCreate.Data?.Id }, userCreate);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> UpdateUserAsync([FromRoute] int id, [FromBody] UpdateUserRequest updateUserRequest)
    {
        ServiceResponse<UserResponse> userUpdate = await _userService.UpdateUserAsync(id, updateUserRequest);
        switch (userUpdate.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: userUpdate.Status.ToString());
            case Status.UpdateError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: userUpdate.Status.ToString());
            default:
                return Ok(userUpdate);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> DeleteUserAsync([FromRoute] int id)
    {
        ServiceResponse<UserResponse> userDelete = await _userService.DeleteUserAsync(id);
        switch (userDelete.Status)
        {
            case Status.NotFound:
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: userDelete.Status.ToString());
            case Status.DeleteError:
                return Problem(statusCode: StatusCodes.Status500InternalServerError, detail: userDelete.Status.ToString());
            default:
                return Ok(userDelete);
        }
    }
}