using FullTractor.Domain.Enums;

namespace FullTractor.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string CellPhone { get; set; }
    public required string Email { get; set; }
    public string? PasswordHash { get; set; }
}