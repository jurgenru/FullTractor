using FullTractor.Domain.Entities;
using FullTractor.Domain.Interfaces;
using FullTractor.Infrastructure.Context;
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
            throw new Exception("Unable to create user. Verify if the properties are correct or the database connection.", updateEx);
        }
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        int numUpdated = await _fullTractorContext.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(setter => setter
                                                                    .SetProperty(u => u.Name, user.Name)
                                                                    .SetProperty(u => u.LastName, user.LastName)
                                                                    .SetProperty(u => u.Address, user.Address)
                                                                    .SetProperty(u => u.CellPhone, user.CellPhone)
                                                                    .SetProperty(u => u.City, user.City));
        if (numUpdated == 0) return null;
        return user;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        int numDeleted = await _fullTractorContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
        if (numDeleted == 0) return false;
        return true;
    }
}