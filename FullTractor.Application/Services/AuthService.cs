using FullTractor.Application.DTOs.Service.Response;
using FullTractor.Application.DTOs.User.Request;
using FullTractor.Application.DTOs.User.Response;
using FullTractor.Application.Enums;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FullTractor.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserRequest> _passwordHasher;
    private readonly ITokenService _tokenService;
    public AuthService(IUserRepository userRepository, IPasswordHasher<UserRequest> passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }
    public async Task<ServiceResponse<LoginResponse>> LoginAsync(UserRequest userRequest)
    {
        User? user = await _userRepository.GetUserByEmailAsync(userRequest.Email);
        if (user == null) return new ServiceResponse<LoginResponse> { Status = Status.NotFound };
        PasswordVerificationResult passwordStatus = _passwordHasher.VerifyHashedPassword(userRequest, user.PasswordHash, userRequest.PasswordHash);
        switch (passwordStatus)
        {
            case PasswordVerificationResult.Failed:
                return new ServiceResponse<LoginResponse> { Status = Status.PasswordIncorrect };
            default:
                (string token, DateTime expiryTime) = _tokenService.CreateToken(user);
                return new ServiceResponse<LoginResponse> { Status = Status.Success, Data = new LoginResponse { Token = token, ExpiryTime = expiryTime } };
        }
    }
}