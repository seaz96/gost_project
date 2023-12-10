using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Users;

public class UsersRepository : IUsersRepository
{
    private readonly DataContext _context;

    public UsersRepository(DataContext context)
    {
        _context = context;
    }

    public List<UserEntity> GetAll()
    {
        return _context.Users.ToList();
    }

    public List<UserEntity> GetByCompany(long id)
    {
        return _context.Users.Where(user => user.CompanyId == id).ToList();
    }

    public void Add(UserEntity user)
    {
        _context.Users.Add(user);
    }

    public void Delete(long id)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        if (user is not null)
        {
            _context.Users.Remove(user);
        }
    }

    public void UpdateName(long id, string? name)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        if (user is not null)
        {
            user.Name = name;
        }
    }
}