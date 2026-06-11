using FullTractor.Application.DTOs;
using FullTractor.Application.DTOs.Service;

namespace FullTractor.Application.Interfaces;

public interface IUserService
{
    public Task<ServiceResponse<UserResponse>> GetUserByIdAsync(int id);
    
}