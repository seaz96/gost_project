using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Concrete;

public interface IFieldsRepository
{
    public Task<List<FieldEntity>> GetAllAsync();

    public Task<FieldEntity?> GetByIdAsync(long? id);

    public Task<long> AddAsync(FieldEntity field);

    public Task DeleteAsync(long? id);

    public Task UpdateAsync(FieldEntity newField);
}