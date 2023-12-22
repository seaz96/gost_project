using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IDocsRepository
{
    public List<DocEntity> GetAll();

    public DocEntity GetById(long id);

    public long Add(DocEntity field);
}