namespace FullTractor.Application.DTOs;

public class UserRequest
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}