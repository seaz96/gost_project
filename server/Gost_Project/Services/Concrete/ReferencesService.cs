using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Services.Abstract;

namespace Gost_Project.Services.Concrete;

public class ReferencesService(IReferencesRepository referencesRepository) : IReferencesService
{
    private readonly IReferencesRepository _referencesRepository = referencesRepository;


    public async Task AddReferencesAsync(List<long> referenceIds, long parentId)
    {
        var references = referenceIds
            .Select(childId => new DocReferenceEntity { ParentalDocId = parentId, ChildDocId = childId })
            .ToList();

        await _referencesRepository.AddRangeAsync(references);
    }

    public async Task DeleteReferencesByIdAsync(long id)
    {
        await _referencesRepository.DeleteAllByParentIdAsync(id);
        await _referencesRepository.DeleteAllByChildIdAsync(id);
    }

    public async Task UpdateReferencesAsync(List<long> referenceIds, long parentId)
    {
        await _referencesRepository.UpdateByParentIdAsync(referenceIds, parentId);
    }
}