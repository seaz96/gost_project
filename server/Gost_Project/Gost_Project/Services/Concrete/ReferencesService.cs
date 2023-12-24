using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Services.Abstract;

namespace Gost_Project.Services.Concrete;

public class ReferencesService : IReferencesService
{
    private readonly IReferencesRepository _referencesRepository;

    public ReferencesService(IReferencesRepository referencesRepository)
    {
        _referencesRepository = referencesRepository;
    }

    public void AddReferences(List<long> referenceIds, long parentId)
    {
        var references = referenceIds
            .Select(childId => new DocReferenceEntity { ParentalDocId = parentId, ChildDocId = childId })
            .ToList();

        _referencesRepository.AddRange(references);
    }

    public void DeleteReferencesById(long id)
    {
        _referencesRepository.DeleteAllByParentId(id);
        _referencesRepository.DeleteAllByChildId(id);
    }

    public void UpdateReferences(List<long> referenceIds, long parentId)
    {
        _referencesRepository.UpdateByParentId(referenceIds, parentId);
    }
}