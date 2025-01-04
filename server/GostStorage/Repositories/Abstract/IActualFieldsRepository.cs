using GostStorage.Entities;

namespace GostStorage.Repositories.Abstract;

public interface IActualFieldsRepository
{
    public Task<List<ActualField>> GetAllAsync();

    public ActualField? GetById(long? id);

    public Task<ICollection<ActualField>> GetFieldsByDocIds(ICollection<long> docIds);

    public Task<ActualField?> GetByIdAsync(long? id);

    public Task<long> AddAsync(ActualField field);

    public Task DeleteAsync(long? id);

    public Task UpdateAsync(ActualField newField);
}