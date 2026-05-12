using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IUserRepository
{
    public Task<List<User>> GetAllUsersAsync();
    public Task<User?> GetUserByIdAsync(int userId);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User> CreateUserAsync(User user);
    public Task<User> UpdateUserAsync(User user);
    public Task<bool> DeleteUserAsync(int userId);
}