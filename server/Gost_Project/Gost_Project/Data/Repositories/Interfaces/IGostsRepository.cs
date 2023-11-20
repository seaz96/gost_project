using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories;

public interface IGostsRepository
{
    public List<GostEntity> GetAll();

    public GostEntity GetById(long id);

    public List<GostEntity> GetByCompany(long companyId);

    public long Add(GostEntity field);
}