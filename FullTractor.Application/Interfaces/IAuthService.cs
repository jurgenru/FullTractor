using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.DTOs.User.Response;

namespace FullTractor.Application.Interfaces;

public interface IAuthService
{
    public Task<ServiceResponse<LoginResponse>> LoginAsync(UserRequest userRequest);
}