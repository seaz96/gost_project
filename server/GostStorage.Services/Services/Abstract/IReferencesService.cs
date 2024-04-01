namespace GostStorage.Services.Services.Abstract;

public interface IReferencesService
{
    public Task AddReferencesAsync(List<long> referenceIds, long parentId);

    public Task DeleteReferencesByIdAsync(long id);

    public Task UpdateReferencesAsync(List<long> referenceIds, long parentId);
}