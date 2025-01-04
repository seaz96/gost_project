using GostStorage.Entities;

namespace GostStorage.Repositories;

public interface IReferencesRepository
{
    public Task<List<DocumentReference>> GetAllAsync();

    public Task<DocumentReference?> GetByIdAsync(long id);

    public Task<DocumentReference?> GetByParentIdAsync(long id);

    public Task<DocumentReference?> GetByChildIdAsync(long id);

    public Task AddAsync(DocumentReference reference);

    public Task AddRangeAsync(List<DocumentReference> references);

    public Task DeleteAllByParentIdAsync(long parentId);

    public Task DeleteAllByChildIdAsync(long parentId);

    public Task UpdateByParentIdAsync(List<long> referenceIds, long parentId);
}