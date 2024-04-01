using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Services.Services.Abstract;

namespace GostStorage.Services.Services.Concrete;

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