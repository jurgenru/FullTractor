namespace FullTractor.Application.DTOs;
public class UpdateUserRequest
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string CellPhone { get; set; }
    public required string Email { get; set; }
}