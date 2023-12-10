using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Fields;

public interface IFieldsRepository
{
    public List<FieldEntity> GetAll();

    public FieldEntity GetById(long id);

    // public List<FieldEntity> GetByGostId(long gostId);

    public long Add(FieldEntity field);
}