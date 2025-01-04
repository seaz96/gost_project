using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Navigations;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class DocumentsRepository(DataContext context) : IDocumentsRepository
{
    public Task<int> GetCountOfDocumentsAsync(GetDocumentRequest? parameters)
    {
        return context.Documents
            .Join(context.PrimaryFields,
                d => d.Id,
                f => f.DocId,
                (d, p) => new { d, p })
            .Join(context.ActualFields,
                g => g.d.Id,
                f => f.DocId,
                (g, a) => new FullDocument
                {
                    DocId = g.d.Id,
                    Actual = a,
                    Primary = g.p,
                    Status = g.d.Status
                })
            .ApplyFilters(parameters)
            .CountAsync();
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

    public Task<FullDocument?> GetDocumentWithFields(long docId)
    {
        return context.Documents
            .Join(context.PrimaryFields,
                d => d.Id,
                f => f.DocId,
                (d, p) => new { d, p })
            .Join(context.ActualFields,
                g => g.d.Id,
                f => f.DocId,
                (g, a) => new FullDocument
                {
                    DocId = g.d.Id,
                    Actual = a,
                    Primary = g.p,
                    Status = g.d.Status
                })
            .Where(x => x.DocId == docId)
            .FirstOrDefaultAsync();
    }
    
    public Task<List<FullDocument>> GetDocumentsWithFields(GetDocumentRequest? parameters)
    {
        return context.Documents
            .Join(context.PrimaryFields,
                d => d.Id,
                f => f.DocId,
                (d, p) => new { d, p })
            .Join(context.ActualFields,
                g => g.d.Id,
                f => f.DocId,
                (g, a) => new FullDocument
                {
                    DocId = g.d.Id,
                    Actual = a,
                    Primary = g.p,
                    Status = g.d.Status
                })
            .ApplyFilters(parameters)
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