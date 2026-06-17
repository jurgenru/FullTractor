using FullTractor.Domain.Entities;

namespace FullTractor.Domain.Interfaces;

public interface IUserRepository
{
    public Task<List<User>> GetAllUsersAsync();
    public Task<User?> GetUserByIdAsync(int id);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<string?> GetPasswordByEmailAsync(string email);
    public Task<User> CreateUserAsync(User user);
    public Task<User?> UpdateUserAsync(int id, User user);
    public Task<bool> DeleteUserAsync(int id);
}