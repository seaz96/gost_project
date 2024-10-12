using GostStorage.API.Entities;
using GostStorage.API.Helpers;
using GostStorage.API.Navigations;
using GostStorage.API.Repositories.Interfaces;
using GostStorage.API.Services.Interfaces;

namespace GostStorage.API.Services.Concrete;

public class ReferencesService(IReferencesRepository referencesRepository, IDocsRepository docsRepository,
    IDocsService docsService) : IReferencesService
{
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IDocsService _docsService = docsService;


    public async Task AddReferencesAsync(List<string> docChildren, long parentId)
    {
        docChildren = docChildren.Select(TextFormattingHelper.FormatDesignation).ToList();
        var existingDocs = await _docsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = _docsService.AddNewDocAsync(new FieldEntity { Designation = designation, Status = DocStatuses.Inactive});
            
            docId.Wait();
            return docId.Result;
        }); 
        
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

    public async Task UpdateReferencesAsync(List<string> docChildren, long parentId)
    {
        docChildren = docChildren.Select(TextFormattingHelper.FormatDesignation).ToList();
        var existingDocs = await _docsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = _docsService.AddNewDocAsync(new FieldEntity { Designation = designation, Status = DocStatuses.Inactive});
            docId.Wait();
            return docId.Result;
        }).ToList(); 
        
        await _referencesRepository.UpdateByParentIdAsync(referenceIds, parentId);
    }
}