using FullTractor.Application.DTOs;
using FullTractor.Application.DTOs.Service;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Application.Enums;
using FullTractor.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FullTractor.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserRequest> _passwordHasher;
    public UserService(IUserRepository userRepository, IPasswordHasher<UserRequest> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    public async Task<ServiceResponse<UserResponse>> GetUserByEmailAsync(string email)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) return new ServiceResponse<UserResponse> { Status = Status.NotFound };
        return ConvertToUserResponse(user);
    }

    public async Task<ServiceResponse<UserResponse>> GetPasswordByEmailAsync(UserRequest userRequest)
    {
        ServiceResponse<UserResponse> userExist = await GetUserByEmailAsync(userRequest.Email);
        if (userExist.Status == Status.NotFound) return userExist;
        string? password = await _userRepository.GetPasswordByEmailAsync(userRequest.Email);
        if (password == null) return new ServiceResponse<UserResponse> { Status = Status.NotFound };
        PasswordVerificationResult passwordStatus = _passwordHasher.VerifyHashedPassword(userRequest, password, userRequest.PasswordHash);
        switch (passwordStatus)
        {
            case PasswordVerificationResult.Failed:
                return new ServiceResponse<UserResponse> { Status = Status.PasswordIncorrect };
            case PasswordVerificationResult.SuccessRehashNeeded:
                return new ServiceResponse<UserResponse> { Status = Status.PasswordIncorrect };
            default:
                return await GetUserByEmailAsync(userRequest.Email);
        }
    }

    public async Task<ServiceResponse<UserResponse>> GetUserByIdAsync(int id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return new ServiceResponse<UserResponse> { Status = Status.NotFound };
        return ConvertToUserResponse(user);
    }

    public async Task<ServiceResponse<UserResponse>> CreateUserAsync(UserRequest userRequest)
    {
        ServiceResponse<UserResponse> userExist = await GetUserByEmailAsync(userRequest.Email);
        if (userExist.Status == Status.NotFound)
        {
            try
            {
                string hashPassword = _passwordHasher.HashPassword(userRequest, userRequest.PasswordHash);
                User user = await _userRepository.CreateUserAsync(new User { Email = userRequest.Email, PasswordHash = hashPassword, Name = "", Address = "", CellPhone = "", LastName = "", City = "" });
                return ConvertToUserResponse(user);
            }
            catch (DuplicateRecordException)
            {
                return new ServiceResponse<UserResponse> { Status = Status.EmailExists };
            }
        }
        return new ServiceResponse<UserResponse> { Status = Status.EmailExists };
    }

    public async Task<ServiceResponse<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest updateUserRequest)
    {
        ServiceResponse<UserResponse> userExists = await GetUserByIdAsync(id);
        if (userExists.Status == Status.NotFound) return userExists;
        User? user = await _userRepository.UpdateUserAsync(id, new User { Name = updateUserRequest.Name, Address = updateUserRequest.Address, CellPhone = updateUserRequest.CellPhone, City = updateUserRequest.City, Email = updateUserRequest.Email, LastName = updateUserRequest.LastName });
        if (user == null) return new ServiceResponse<UserResponse> { Status = Status.UpdateError };
        return ConvertToUserResponse(user);
    }

    public async Task<ServiceResponse<UserResponse>> DeleteUserAsync(int id)
    {
        ServiceResponse<UserResponse> userExists = await GetUserByIdAsync(id);
        if (userExists.Status == Status.NotFound) return userExists;
        bool userDelete = await _userRepository.DeleteUserAsync(id);
        if (!userDelete) return new ServiceResponse<UserResponse> { Status = Status.DeleteError };
        return new ServiceResponse<UserResponse> { Status = Status.Success };
    }

    private ServiceResponse<UserResponse> ConvertToUserResponse(User user)
    {
        return new ServiceResponse<UserResponse> { Status = Status.Success, Data = new UserResponse { Id = user.Id, Name = user.Name, LastName = user.LastName, City = user.City, Address = user.Address, CellPhone = user.CellPhone, Email = user.Email } };
    }
}