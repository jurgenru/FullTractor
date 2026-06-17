using FullTractor.Domain.Entities;
using FullTractor.Domain.Exceptions;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace FullTractor.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FullTractorContext _fullTractorContext;
    public UserRepository(FullTractorContext fullTractorContext)
    {
        _fullTractorContext = fullTractorContext;
    }
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _fullTractorContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _fullTractorContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _fullTractorContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<string?> GetPasswordByEmailAsync(string email)
    {
        return await _fullTractorContext.Users.AsNoTracking().Where(u => u.Email == email).Select(u => u.PasswordHash).FirstOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _fullTractorContext.Users.Add(user);
            await _fullTractorContext.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateException updateEx)
        {
            if (updateEx.InnerException is SqlException upEx && (upEx.Number == 2627 || upEx.Number == 2601))
            {
                throw new DuplicateRecordException($"{user.Email} already exist");
            }
            throw;
        }
    }

    public async Task<User?> UpdateUserAsync(int id, User user)
    {
        int numUpdated = await _fullTractorContext.Users.Where(u => u.Id == id).ExecuteUpdateAsync(setter => setter
                                                                    .SetProperty(u => u.Name, user.Name)
                                                                    .SetProperty(u => u.LastName, user.LastName)
                                                                    .SetProperty(u => u.Address, user.Address)
                                                                    .SetProperty(u => u.CellPhone, user.CellPhone)
                                                                    .SetProperty(u => u.City, user.City));
        if (numUpdated == 0) return null;
        return user;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        int numDeleted = await _fullTractorContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
        if (numDeleted == 0) return false;
        return true;
    }
}