using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IUsersRepository
{
    public List<UserEntity> GetAll();

    public void Add(UserEntity user);

    public void Delete(long id);

    public void UpdateName(long id, string? name);
}