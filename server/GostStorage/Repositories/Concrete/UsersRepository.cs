using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class UsersRepository(DataContext context) : IUsersRepository
{
    public async Task<List<UserEntity>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<UserEntity?> GetUserAsync(string login)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<UserEntity?> GetUserAsync(long id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(UserEntity user)
    {
        await context.Users.AddAsync(user);

        await context.SaveChangesAsync();
    }

    public async Task<bool> IsLoginExistAsync(string login)
    {
        return await context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user is not null)
        {
            context.Users.Remove(user);
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateNameAsync(long id, string? name)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
        
        if (user is not null)
        {
            user.Name = name;
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdatePasswordAsync(long id, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user is not null)
        {
            user.Password = password;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(UserEntity user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}