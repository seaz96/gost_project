using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IUsersRepository
{
    public Task<List<UserEntity>> GetAll();

    public Task AddAsync(UserEntity user);

    public Task DeleteAsync(long id);

    public Task UpdateNameAsync(long id, string? name);
}