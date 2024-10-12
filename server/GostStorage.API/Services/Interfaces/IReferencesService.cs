namespace GostStorage.API.Services.Interfaces;

public interface IReferencesService
{
    public Task AddReferencesAsync(List<string> docChildren, long parentId);

    public Task DeleteReferencesByIdAsync(long id);

    public Task UpdateReferencesAsync(List<string> docChildren, long parentId);
}