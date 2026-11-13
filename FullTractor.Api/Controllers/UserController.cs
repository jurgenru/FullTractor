using FullTractor.Api.Extensions;
using FullTractor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Response;
using FullTractor.Application.DTOs.User.Request;
using Microsoft.AspNetCore.Authorization;

namespace FullTractor.Api.Controllers;

[Route("[controller]")]
[Authorize]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> GetUserByIdAsync([FromRoute] int id)
    {
        if (!User.CanAccessUser(id)) return Forbid();
        return ToResponse(await _userService.GetUserByIdAsync(id));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> GetUserByEmailAsync([FromQuery] string email) =>
        ToResponse(await _userService.GetUserByEmailAsync(email));

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> CreateUserAsync([FromBody] UserRequest userRequest)
    {
        ServiceResponse<UserResponse> userCreate = await _userService.CreateUserAsync(userRequest);
        if (userCreate.Data == null) return ToProblem(userCreate.Status);
        return CreatedAtAction("GetUserById", new { id = userCreate.Data.Id }, userCreate);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> UpdateUserAsync([FromRoute] int id, [FromBody] UpdateUserRequest updateUserRequest) =>
        ToResponse(await _userService.UpdateUserAsync(id, updateUserRequest));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<UserResponse>>> DeleteUserAsync([FromRoute] int id) =>
        ToResponse(await _userService.DeleteUserAsync(id));
}
