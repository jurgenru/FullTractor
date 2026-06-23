using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.DTOs.User.Response;

namespace FullTractor.Application.Interfaces;

public interface IUserService
{
    public Task<ServiceResponse<UserResponse>> GetUserByIdAsync(int id);
    public Task<ServiceResponse<UserResponse>> GetUserByEmailAsync(string email);
    public Task<ServiceResponse<UserResponse>> CreateUserAsync(UserRequest userRequest);
    public Task<ServiceResponse<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest updateUserRequest);
    public Task<ServiceResponse<UserResponse>> DeleteUserAsync(int id);
}