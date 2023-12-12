using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Gosts;

public interface IGostsRepository
{
    public List<GostEntity> GetAll();

    public GostEntity GetById(long id);

    public long Add(GostEntity field);
}