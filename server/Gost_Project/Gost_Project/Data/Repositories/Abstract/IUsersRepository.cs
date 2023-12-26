using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IUsersRepository
{
    public Task<List<UserEntity>> GetAllAsync();

    public Task AddAsync(UserEntity user);

    public Task DeleteAsync(long id);

    public Task UpdateNameAsync(long id, string? name);

    public Task<bool> IsLoginExistAsync(string login);

    public Task<UserEntity?> GetUserAsync(string login);
}