using GostStorage.API.Data;
using GostStorage.API.Entities;
using GostStorage.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.API.Repositories.Concrete;

public class UsersRepository(DataContext context) : IUsersRepository
{
    private readonly DataContext _context = context;

    public async Task<List<UserEntity>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserEntity?> GetUserAsync(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<UserEntity?> GetUserAsync(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsLoginExistAsync(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user is not null)
        {
            _context.Users.Remove(user);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateNameAsync(long id, string? name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        
        if (user is not null)
        {
            user.Name = name;
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdatePasswordAsync(long id, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is not null)
        {
            user.Password = password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}