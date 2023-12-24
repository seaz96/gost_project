namespace Gost_Project.Services.Abstract;

public interface IReferencesService
{
    public void AddReferences(List<long> referenceIds, long parentId);

    public void DeleteReferencesById(long id);

    public void UpdateReferences(List<long> referenceIds, long parentId);
}