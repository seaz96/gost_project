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

    public void AddReferences(List<long> referencesIds, long parentId)
    {
        var references = referencesIds
            .Select(childId => new DocReferenceEntity { ParentalDocId = parentId, ChildDocId = childId })
            .ToList();

        _referencesRepository.AddRange(references);
    }
}