using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Gost_Project.Data.Repositories.Concrete;

public class UsersRepository(DataContext context) : IUsersRepository
{
    private readonly DataContext _context = context;

    public async Task<List<UserEntity>> GetAll()
    {
        return [.. _context.Users];
    }

    public async Task AddAsync(UserEntity user)
    {
        _context.Users.Add(user);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is not null)
        {
            _context.Users.Remove(user);
        }
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
}