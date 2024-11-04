using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Navigations;
using GostStorage.Repositories.Interfaces;
using GostStorage.Services.Interfaces;

namespace GostStorage.Services.Concrete;

public class ReferencesService(
        IReferencesRepository referencesRepository,
        IDocsRepository docsRepository,
        IDocsService docsService)
    : IReferencesService
{
    public async Task AddReferencesAsync(List<string> docChildren, long parentId)
    {
        docChildren = docChildren.Select(TextFormattingHelper.FormatDesignation).ToList();
        var existingDocs = await docsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = docsService.AddNewDocAsync(new FieldEntity { Designation = designation, Status = DocStatuses.Inactive});
            
            docId.Wait();
            return docId.Result;
        }); 
        
        var references = referenceIds
            .Select(childId => new DocReferenceEntity { ParentalDocId = parentId, ChildDocId = childId })
            .ToList();

        await referencesRepository.AddRangeAsync(references);
    }

    public async Task DeleteReferencesByIdAsync(long id)
    {
        await referencesRepository.DeleteAllByParentIdAsync(id);
        await referencesRepository.DeleteAllByChildIdAsync(id);
    }

    public async Task UpdateReferencesAsync(List<string> docChildren, long parentId)
    {
        docChildren = docChildren.Select(TextFormattingHelper.FormatDesignation).ToList();
        var existingDocs = await docsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = docsService.AddNewDocAsync(new FieldEntity { Designation = designation, Status = DocStatuses.Inactive});
            docId.Wait();
            return docId.Result;
        }).ToList(); 
        
        await referencesRepository.UpdateByParentIdAsync(referenceIds, parentId);
    }
}