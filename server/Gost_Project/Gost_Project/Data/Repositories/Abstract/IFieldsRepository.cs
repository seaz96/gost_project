using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Concrete;

public interface IFieldsRepository
{
    public List<FieldEntity> GetAll();

    public FieldEntity? GetById(long? id);

    // public List<FieldEntity> GetByGostId(long gostId);

    public long Add(FieldEntity field);

    public void Delete(long? id);

    public void Update(FieldEntity newField);
}