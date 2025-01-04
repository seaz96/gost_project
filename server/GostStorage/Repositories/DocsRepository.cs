using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class DocsRepository(DataContext context) : IDocsRepository
{
    public async Task<List<Document>> GetAllAsync()
    {
        return await context.Documents.ToListAsync();
    }

    public async Task<List<Document>> GetDocumentsAsync(GetDocumentRequest parameters)
    {
        var fieldIds = await SearchHelper.SearchFields(parameters, context);

        var docs = context.Documents
            .Where(x => fieldIds.Contains(x.PrimaryFieldId) || fieldIds.Contains(x.ActualFieldId))
            .Distinct()
            .OrderBy(x => x.Id)
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToList();
        
        return docs;
    }
    
    public async Task<int> GetCountOfDocumentsAsync(GetDocumentRequest parameters)
    {
        var fieldIds = await SearchHelper.SearchFields(parameters, context);

        return context.Documents
            .Where(x => fieldIds.Contains(x.PrimaryFieldId) || fieldIds.Contains(x.ActualFieldId))
            .Distinct()
            .Count();
    }

    public Task<Document?> GetByIdAsync(long id)
    {
        return context.Documents.FirstOrDefaultAsync(doc => doc.Id == id);
    }

    public Task<Document?> GetByDesignationAsync(string designation)
    {
        return context.Documents.Where(x => x.Designation == designation).FirstOrDefaultAsync();
    }

    public async Task<IList<DocWithGeneralInfoModel>> GetDocsIdByDesignationAsync(List<string> docDesignations)
    {
        return await context.Documents
            .Where(doc => docDesignations.Contains(doc.Designation))
            .Select(doc => new DocWithGeneralInfoModel
                {
                    Id = doc.Id,
                    Designation = doc.Designation
                })
            .AsSingleQuery()
            .ToListAsync();
    }

    public async Task<long> AddAsync(Document document)
    {
        await context.Documents.AddAsync(document);
        await context.SaveChangesAsync();
        return document.Id;
    }

    public async Task DeleteAsync(long id)
    {
        await context.Documents.Where(doc => doc.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateStatusAsync(long id, DocumentStatus status)
    {
        var document = await context.Documents.FirstOrDefaultAsync(d => d.Id == id);

        if (document is null) return;
        
        document.Status = status;
        context.Documents.Update(document);
        await context.SaveChangesAsync();
    }
}