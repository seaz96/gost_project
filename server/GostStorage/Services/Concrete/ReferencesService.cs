using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Navigations;
using GostStorage.Repositories.Abstract;
using GostStorage.Services.Abstract;

namespace GostStorage.Services.Concrete;

public class ReferencesService(
        IReferencesRepository referencesRepository,
        IDocumentsRepository documentsRepository,
        IDocumentsService documentsService)
    : IReferencesService
{
    public async Task AddReferencesAsync(List<string> docChildren, long parentId)
    {
        docChildren = docChildren.Select(TextFormattingHelper.FormatDesignation).ToList();
        var existingDocs = await documentsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = documentsService.AddDocumentAsync(new PrimaryField { Designation = designation}, DocumentStatus.Inactive);
            
            docId.Wait();
            return docId.Result;
        }); 
        
        var references = referenceIds
            .Select(childId => new DocumentReference { ParentalDocId = parentId, ChildDocId = childId })
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
        var existingDocs = await documentsRepository.GetDocsIdByDesignationAsync(docChildren);

        var referenceIds = docChildren.Select(designation =>
        {
            if (existingDocs.Any(x => x.Designation == designation))
            {
                return existingDocs.First(x => x.Designation == designation).Id;
            }

            var docId = documentsService
                .AddDocumentAsync(new PrimaryField { Designation = designation }, DocumentStatus.Inactive)
                .GetAwaiter().GetResult();
            return docId;
        }).ToList(); 
        
        await referencesRepository.UpdateByParentIdAsync(referenceIds, parentId);
    }
}