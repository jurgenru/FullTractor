namespace FullTractor.Application.DTOs.User.Request;

public class UserRequest
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}