using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IFieldsRepository
{
    public Task<List<FieldEntity>> GetAllAsync();

    public FieldEntity? GetById(long? id);

    public Task<ICollection<FieldEntity>> GetFieldsByDocIds(ICollection<long> docIds);
    
    public Task<FieldEntity?> GetByIdAsync(long? id);

    public Task<long> AddAsync(FieldEntity field);

    public Task DeleteAsync(long? id);

    public Task UpdateAsync(FieldEntity newField);
}