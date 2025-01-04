using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IPrimaryFieldsRepository
{
    public Task<List<PrimaryField>> GetAllAsync();

    public PrimaryField? GetById(long? id);

    public Task<ICollection<PrimaryField>> GetFieldsByDocIds(ICollection<long> docIds);

    public Task<PrimaryField?> GetByIdAsync(long? id);

    public Task<long> AddAsync(PrimaryField field);

    public Task DeleteAsync(long? id);

    public Task UpdateAsync(PrimaryField newField);
}