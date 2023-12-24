namespace Gost_Project.Services.Abstract;

public interface IReferencesService
{
    public void AddReferences(List<long> referencesIds, long parentId);
}