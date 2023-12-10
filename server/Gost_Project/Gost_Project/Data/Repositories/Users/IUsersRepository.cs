using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Users;

public interface IUsersRepository
{
    public List<UserEntity> GetAll();

    public List<UserEntity> GetByCompany(long id);

    public void Add(UserEntity user);

    public void Delete(long id);

    public void UpdateName(long id, string? name);
}