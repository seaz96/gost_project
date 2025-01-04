using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IFieldsRepository
{
    public Task<List<Field>> GetAllAsync();

    public Field? GetById(long? id);

    public Task<ICollection<Field>> GetFieldsByDocIds(ICollection<long> docIds);

    public Task<Field?> GetByIdAsync(long? id);

    public Task<long> AddAsync(Field field);

    public Task DeleteAsync(long? id);

    public Task UpdateAsync(Field newField);
}