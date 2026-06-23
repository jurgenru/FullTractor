namespace FullTractor.Application.DTOs.User.Response;

public class LoginResponse
{
    public required string Token { get; set; }
    public required DateTime ExpiryTime { get; set; }
}