namespace FullTractor.Application.DTOs.User.Response;

public class UserResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string CellPhone { get; set; }
    public required string Email { get; set; }
}