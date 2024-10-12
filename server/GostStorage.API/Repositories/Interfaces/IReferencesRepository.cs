using GostStorage.API.Entities;

namespace GostStorage.API.Repositories.Interfaces;

public interface IReferencesRepository
{
    public Task<List<DocReferenceEntity>> GetAllAsync();

    public Task<DocReferenceEntity?> GetByIdAsync(long id);

    public Task<DocReferenceEntity?> GetByParentIdAsync(long id);

    public Task<DocReferenceEntity?> GetByChildIdAsync(long id);

    public Task AddAsync(DocReferenceEntity reference);

    public Task AddRangeAsync(List<DocReferenceEntity> references);

    public Task DeleteAllByParentIdAsync(long parentId);

    public Task DeleteAllByChildIdAsync(long parentId);

    public Task UpdateByParentIdAsync(List<long> referenceIds, long parentId);
}