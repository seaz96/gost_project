using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IUsersRepository
{
    public Task<List<User>> GetAllAsync();

    public Task AddAsync(User user);

    public Task DeleteAsync(long id);

    public Task UpdateNameAsync(long id, string? name);

    public Task<bool> IsLoginExistAsync(string login);

    public Task<User?> GetUserAsync(string login);

    public Task UpdatePasswordAsync(long id, string password);

    public Task UpdateAsync(User user);

    public Task<User?> GetUserAsync(long id);
}